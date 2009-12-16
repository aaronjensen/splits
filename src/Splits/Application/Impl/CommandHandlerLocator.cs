using System;
using Microsoft.Practices.ServiceLocation;
using System.Linq;

namespace Splits.Application.Impl
{
  public class CommandHandlerLocator : ICommandHandlerLocator
  {
    class Wrapper<TCommand, TResult> : ICommandHandler<ICommand<ICommandResult>, ICommandResult> 
      where TCommand : ICommand<TResult> 
      where TResult : ICommandResult
    {
      readonly ICommandHandler<TCommand, TResult> _handler;

      public Wrapper(ICommandHandler<TCommand, TResult> handler)
      {
        _handler = handler;
      }

      public TResult Handle(ICommand<TResult> command)
      {
        return _handler.Handle((TCommand)command);
      }

      public ICommandResult Handle(ICommand<ICommandResult> command)
      {
        var wrapper = (CommandWrapper<TCommand>)command;

        return _handler.Handle(wrapper.InnerCommand);
      }
    }

    class CommandWrapper<TCommand> : ICommand<ICommandResult>
    {
      public TCommand InnerCommand { get; private set; }

      public CommandWrapper(object command)
      {
        InnerCommand = (TCommand)command;
      }
    }

    public ICommand<ICommandResult> WrapCommand(object command)
    {
      var commandWrapperType = typeof(CommandWrapper<>).MakeGenericType(command.GetType());

      return (ICommand<ICommandResult>)Activator.CreateInstance(commandWrapperType, command);
    }

    public ICommandHandler<ICommand<ICommandResult>, ICommandResult> LocateHandler(object command)
    {
      if (command == null) throw new ArgumentNullException("command");

      var resultType = command.GetType().GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommand<>)).First().GetGenericArguments().First();
      var handlerType = typeof (ICommandHandler<,>).MakeGenericType(command.GetType(), resultType);
      var handler = ServiceLocator.Current.GetInstance(handlerType);

      var wrapperType = typeof(Wrapper<,>).MakeGenericType(command.GetType(), resultType);

      return (ICommandHandler<ICommand<ICommandResult>, ICommandResult>)Activator.CreateInstance(wrapperType, handler);
    }
  }
}
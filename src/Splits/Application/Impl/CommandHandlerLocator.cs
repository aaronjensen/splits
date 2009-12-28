using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;

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

    public Func<object, ICommandResult> LocateHandler(Type commandType)
    {
      if (commandType == null) throw new ArgumentNullException("commandType");

      var resultType = commandType.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommand<>)).First().GetGenericArguments().First();
      var handlerType = typeof (ICommandHandler<,>).MakeGenericType(commandType, resultType);
      var handler = ServiceLocator.Current.GetInstance(handlerType);

      var wrapperType = typeof(Wrapper<,>).MakeGenericType(commandType, resultType);
      var commandWrapperType = typeof(CommandWrapper<>).MakeGenericType(commandType);

      return x => {
        var wrappedHandler = ((ICommandHandler<ICommand<ICommandResult>, ICommandResult>)
          Activator.CreateInstance(wrapperType, handler));
        var wrappedCommand = (ICommand<ICommandResult>)Activator.CreateInstance(commandWrapperType, x);
        return wrappedHandler.Handle(wrappedCommand);
      };
    }
  }
}
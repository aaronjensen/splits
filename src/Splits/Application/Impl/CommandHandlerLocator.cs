using System;
using Microsoft.Practices.ServiceLocation;

namespace Splits.Application.Impl
{
  public class CommandHandlerLocator : ICommandHandlerLocator
  {
    class Wrapper<TCommand, TResult> : ICommandHandler<ICommand<TResult>, TResult> 
      where TCommand: ICommand<TResult>
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
    }

    public ICommandHandler<ICommand<TResult>, TResult> LocateHandler<TResult>(ICommand<TResult> command) where TResult : ICommandResult
    {
      if (command == null) throw new ArgumentNullException("command");

      var handlerType = typeof (ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
      var handler = ServiceLocator.Current.GetInstance(handlerType);

      var wrapperType = typeof(Wrapper<,>).MakeGenericType(command.GetType(), typeof(TResult));
      return (ICommandHandler<ICommand<TResult>, TResult>)Activator.CreateInstance(wrapperType, handler);
    }
  }
}
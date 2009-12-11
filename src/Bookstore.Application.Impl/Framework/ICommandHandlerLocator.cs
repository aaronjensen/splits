using Bookstore.Application.Framework;

namespace Bookstore.Application.Impl.Framework
{
  public interface ICommandHandlerLocator
  {
    ICommandHandler<ICommand<TResult>, TResult> LocateHandler<TResult>(ICommand<TResult> command)
      where TResult : ICommandResult;
  }
}
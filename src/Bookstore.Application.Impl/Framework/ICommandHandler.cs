using Bookstore.Application.Framework;

namespace Bookstore.Application.Impl.Framework
{
  public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : ICommandResult
  {
    TResult Handle(TCommand command);
  }
}
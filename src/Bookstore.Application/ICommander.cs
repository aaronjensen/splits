using Bookstore.Application.Framework;

namespace Bookstore.Application
{
  public interface ICommander
  {
    TResult Do<TResult>(ICommand<TResult> command) where TResult : ICommandResult;
  }
}
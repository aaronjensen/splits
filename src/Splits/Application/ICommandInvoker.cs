namespace Splits.Application
{
  public interface ICommandInvoker
  {
    TResult Do<TResult>(ICommand<TResult> command) where TResult : ICommandResult;
  }
}
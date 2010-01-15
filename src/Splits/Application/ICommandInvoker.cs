namespace Splits.Application
{
  public interface ICommandInvoker
  {
    ICommandResult Invoke(object command);
    R Invoke<R>(ICommand<R> command);
  }
}
namespace Splits.Application
{
  public interface ICommandInvoker
  {
    ICommandResult Invoke(object command);
  }
}
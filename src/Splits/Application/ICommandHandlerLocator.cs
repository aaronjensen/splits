namespace Splits.Application
{
  public interface ICommandHandlerLocator
  {
    ICommandHandler<ICommand<TResult>, TResult> LocateHandler<TResult>(ICommand<TResult> command)
      where TResult : ICommandResult;
  }
}
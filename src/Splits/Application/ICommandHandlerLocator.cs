namespace Splits.Application
{
  public interface ICommandHandlerLocator
  {
    ICommand<ICommandResult> WrapCommand(object command);
    ICommandHandler<ICommand<ICommandResult>, ICommandResult> LocateHandler(object command);
  }
}
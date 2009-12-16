namespace Splits.Application
{
  public interface ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
    where TResult : ICommandResult
  {
    TResult Handle(TCommand command);
  }
}
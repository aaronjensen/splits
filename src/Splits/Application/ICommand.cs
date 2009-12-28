namespace Splits.Application
{
  public interface ICommand
  {
  }

  public interface ICommand<TResult> : ICommand
  {
  }
}
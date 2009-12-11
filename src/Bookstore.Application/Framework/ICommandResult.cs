namespace Bookstore.Application.Framework
{
  public interface ICommandResult
  {
    bool WasSuccessful { get; }
  }
}
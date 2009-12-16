namespace Splits.Application
{
  public class CommandResult : ICommandResult
  {
    public bool WasSuccessful { get; private set; }
    public CommandResult() : this(true)
    {
    }

    public CommandResult(bool successful)
    {
      WasSuccessful = successful;
    }
  }
}
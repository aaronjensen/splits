namespace Splits.Application
{
  public class CommandResult : ICommandResult
  {
    public bool WasSuccessful { get; private set; }

    public CommandResult()
      : this(true)
    {
    }

    public CommandResult(bool successful)
    {
      WasSuccessful = successful;
    }

    public static CommandResult Success = new CommandResult(true);
    public static CommandResult Failure = new CommandResult(false);
  }
}
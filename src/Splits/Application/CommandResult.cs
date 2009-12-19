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

    public static ICommandResult Success = new CommandResult(true);
    public static ICommandResult Failure = new CommandResult(false);
  }
}
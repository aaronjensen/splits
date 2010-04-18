namespace Splits.Application
{
  public interface ICommandInvoker
  {
    CommandInvocation<ICommandResult> Invoke(object command);
    CommandInvocation<R> Invoke<R>(ICommand<R> command);
  }

  public class CommandInvocation<T>
  {
    public IDomainEvent[] DomainEvents { get; private set; }
    public T Result { get; private set; }

    public CommandInvocation(IDomainEvent[] domainEvents, T result)
    {
      DomainEvents = domainEvents;
      Result = result;
    }

    public static implicit operator T (CommandInvocation<T> invocation)
    {
      return invocation.Result;
    }
  }
}
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Linq;

namespace Splits.Application.Impl
{
  public class CommandInvoker : ICommandInvoker
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(CommandInvoker));
    readonly ICommandHandlerLocator _locator;

    public CommandInvoker(ICommandHandlerLocator locator)
    {
      _locator = locator;
    }

    public CommandInvocation<ICommandResult> Invoke(object command)
    {
      if (command == null) throw new ArgumentNullException("command");
      
      var handler = _locator.LocateHandler(command.GetType());
      if (handler == null) throw new InvalidOperationException("No ICommandHandler for " + command.GetType());

      using (var scope = new TransactionScope())
      {
        DomainEvent.Begin();
        _log.Debug(command);
        var result = handler(command);
        var domainEvents = DomainEvent.Commit();
        scope.Complete();
        return new CommandInvocation<ICommandResult>(domainEvents.ToArray(), result);
      }
    }

    public CommandInvocation<R> Invoke<R>(ICommand<R> command)
    {
      var invocation = Invoke((object) command);
      return new CommandInvocation<R>(invocation.DomainEvents, (R)invocation.Result);
    }
  }
}
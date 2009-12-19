using System;
using System.Collections.Generic;
using System.Transactions;

namespace Splits.Application.Impl
{
  public class CommandInvoker : ICommandInvoker
  {
    readonly ICommandHandlerLocator _locator;

    public CommandInvoker(ICommandHandlerLocator locator)
    {
      _locator = locator;
    }

    public ICommandResult Invoke(object command)
    {
      if (command == null) throw new ArgumentNullException("command");
      
      var handler = _locator.LocateHandler(command.GetType());
      if (handler == null) throw new InvalidOperationException("No ICommandHandler for " + command.GetType());

      using (var scope = new TransactionScope())
      {
        var result = handler(command);
        scope.Complete();
        return result;
      }
    }
  }
}
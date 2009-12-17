using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

      return handler(command);
    }
  }
}
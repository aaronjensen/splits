using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Framework;

namespace Bookstore.Application.Impl.Framework.Impl
{
  public class CommandInvoker : ICommandInvoker
  {
    readonly ICommandHandlerLocator _locator;

    public CommandInvoker(ICommandHandlerLocator locator)
    {
      _locator = locator;
    }

    public TResult Do<TResult>(ICommand<TResult> command) where TResult : ICommandResult
    {
      if (command == null) throw new ArgumentNullException("command");

      var handler = _locator.LocateHandler(command);
      if (handler == null) throw new InvalidOperationException("No ICommandHandler for " + command.GetType());

      return handler.Handle(command);
    }
  }
}
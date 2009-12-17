using System;

namespace Splits.Application
{
  public interface ICommandHandlerLocator
  {
    Func<object, ICommandResult> LocateHandler(object command);
  }
}
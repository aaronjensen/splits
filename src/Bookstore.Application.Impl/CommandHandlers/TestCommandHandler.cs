using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Commands;
using Splits;
using Splits.Application;

namespace Bookstore.Application.Impl.CommandHandlers
{
  public class TestCommandHandler : ICommandHandler<TestCommand, CommandResult>
  {
    public CommandResult Handle(TestCommand command)
    {
      if (command.Foo > 0)
        return new CommandResult(true);

      return new CommandResult(false);
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splits.Application;

namespace Bookstore.Application.Commands
{
  public class BookStoreSetupCommand : ICommand<CommandResult>
  {
    public string Name { get; set; }
  }
}

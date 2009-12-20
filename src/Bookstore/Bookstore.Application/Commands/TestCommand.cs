using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Splits.Application;

namespace Bookstore.Application.Commands
{
  public class TestCommand : ICommand<CommandResult>
  {
    [Required]
    public int? Foo { get; set; }
  }
}

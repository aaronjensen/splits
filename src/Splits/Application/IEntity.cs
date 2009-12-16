using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splits.Application
{
  public interface IEntity
  {
    Guid Id { get; }
  }
}
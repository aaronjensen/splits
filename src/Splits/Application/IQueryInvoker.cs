using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splits.Application
{
  public interface IQueryInvoker
  {
    TResult Get<TResult>(IQuery<TResult> query);
  }
}
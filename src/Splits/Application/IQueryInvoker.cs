using System;
using System.Collections.Generic;

namespace Splits.Application
{
  public interface IQueryInvoker
  {
    object Invoke(object query);
    R Invoke<R>(IQuery<R> query);
  }
}
using System;
using Splits;

namespace Splits.Application
{
  public interface IQueryHandlerLocator
  {
    Func<object, object> LocateHandler(Type queryType);
  }
}
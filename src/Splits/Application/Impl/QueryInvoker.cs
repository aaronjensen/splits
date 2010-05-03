using System;
using Splits;

namespace Splits.Application.Impl
{
  public class QueryInvoker : IQueryInvoker
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(QueryInvoker));
    readonly IQueryHandlerLocator _locator;

    public QueryInvoker(IQueryHandlerLocator locator)
    {
      _locator = locator;
    }

    public object Invoke(object query)
    {
      if (query == null) throw new ArgumentNullException("query");

      var handler = _locator.LocateHandler(query.GetType());
      if (handler == null) throw new InvalidOperationException("No IQueryHandler for " + query.GetType());

      _log.Debug(query);
      return handler(query);
    }

    public R Invoke<R>(IQuery<R> query)
    {
      return (R)Invoke((object)query);
    }
  }
}
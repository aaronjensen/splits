using System;
using Splits;

namespace Splits.Application.Impl
{
  public class QueryInvoker : IQueryInvoker
  {
    readonly IQueryHandlerLocator _locator;

    public QueryInvoker(IQueryHandlerLocator locator)
    {
      _locator = locator;
    }

    public TResult Get<TResult>(IQuery<TResult> query)
    {
      if (query == null) throw new ArgumentNullException("query");

      var handler = _locator.LocateHandler(query);
      if (handler == null) throw new InvalidOperationException("No IQueryHandler for " + query.GetType());

      return handler.Handle(query);
    }
  }
}
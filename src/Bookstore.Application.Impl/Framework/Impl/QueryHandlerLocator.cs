using System;
using Bookstore.Application.Framework;
using Microsoft.Practices.ServiceLocation;

namespace Bookstore.Application.Impl.Framework.Impl
{
  public class QueryHandlerLocator : IQueryHandlerLocator
  {
    class Wrapper<TQuery, TResult> : IQueryHandler<IQuery<TResult>, TResult> 
      where TQuery: IQuery<TResult>
    {
      readonly IQueryHandler<TQuery, TResult> _handler;

      public Wrapper(IQueryHandler<TQuery, TResult> handler)
      {
        _handler = handler;
      }

      public TResult Handle(IQuery<TResult> command)
      {
        return _handler.Handle((TQuery)command);
      }
    }

    public IQueryHandler<IQuery<TReport>, TReport> LocateHandler<TReport>(IQuery<TReport> query)
    {
      if (query == null) throw new ArgumentNullException("query");

      var handlerType = typeof (IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TReport));
      var handler = ServiceLocator.Current.GetInstance(handlerType);

      var wrapperType = typeof(Wrapper<,>).MakeGenericType(query.GetType(), typeof(TReport));
      return (IQueryHandler<IQuery<TReport>, TReport>)Activator.CreateInstance(wrapperType, handler);
    }
  }
}
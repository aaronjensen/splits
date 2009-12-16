using System;
using Microsoft.Practices.ServiceLocation;
using Splits;

namespace Splits.Application.Impl
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

    public IQueryHandler<IQuery<TResult>, TResult> LocateHandler<TResult>(IQuery<TResult> query)
    {
      if (query == null) throw new ArgumentNullException("query");

      var handlerType = typeof (IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
      var handler = ServiceLocator.Current.GetInstance(handlerType);

      var wrapperType = typeof(Wrapper<,>).MakeGenericType(query.GetType(), typeof(TResult));
      return (IQueryHandler<IQuery<TResult>, TResult>)Activator.CreateInstance(wrapperType, handler);
    }
  }
}
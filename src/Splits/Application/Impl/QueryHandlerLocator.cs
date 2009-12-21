using System;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Splits;

namespace Splits.Application.Impl
{
  public class QueryHandlerLocator : IQueryHandlerLocator
  {
    class Wrapper<TQuery, TResult> : IQueryHandler<IQuery<object>, object> 
      where TQuery: IQuery<TResult>
    {
      readonly IQueryHandler<TQuery, TResult> _handler;

      public Wrapper(IQueryHandler<TQuery, TResult> handler)
      {
        _handler = handler;
      }

      public object Handle(IQuery<object> query)
      {
        return _handler.Handle(((QueryWrapper<TQuery>)query).InnerQuery);
      }
    }

    class QueryWrapper<TQuery> : IQuery<object>
      where TQuery : IQuery
    {
      public TQuery InnerQuery { get; private set; }

      public QueryWrapper(object query)
      {
        InnerQuery = (TQuery)query;
      }

      public Guid QueryId
      {
        get { return InnerQuery.QueryId; }
      }
    }

    public Func<object, object> LocateHandler(Type queryType)
    {
      if (queryType == null) throw new ArgumentNullException("queryType");

      var resultType = queryType.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQuery<>)).First().GetGenericArguments().First();
      var handlerType = typeof (IQueryHandler<,>).MakeGenericType(queryType, resultType);
      var handler = ServiceLocator.Current.GetInstance(handlerType);

      var wrapperType = typeof(Wrapper<,>).MakeGenericType(queryType, resultType);
      var queryWrapperType = typeof(QueryWrapper<>).MakeGenericType(queryType);

      return x =>
      {
        var wrappedHandler = ((IQueryHandler<IQuery<object>, object>)
          Activator.CreateInstance(wrapperType, handler));
        var wrappedQuery = (IQuery<object>)Activator.CreateInstance(queryWrapperType, x);

        return wrappedHandler.Handle(wrappedQuery);
      };
    }
  }
}
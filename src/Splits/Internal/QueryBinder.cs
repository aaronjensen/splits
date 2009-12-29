using System;
using System.Collections.Generic;
using Splits.Application;
using Splits.Web;

namespace Splits.Internal
{
  public class QueryBinder : IQueryBinder
  {
    public ICommand Bind(ICommand command, StepContext stepContext)
    {
      Bind(new[] { command }, stepContext);
      return command;
    }

    public IQuery Bind(IQuery query, StepContext stepContext)
    {
      Bind(new[] { query }, stepContext);
      return query;
    }

    void Bind(IEnumerable<object> queries, StepContext stepContext)
    {
      foreach (var query in queries)
      {
        foreach (var queryResult in stepContext.QueryResultMap)
        {
          var binderType = typeof(IBind<,>).MakeGenericType(queryResult.Value.GetType(), query.GetType());
          var wrapperType = typeof(BindWrapper<,>).MakeGenericType(queryResult.Value.GetType(), query.GetType());
          foreach (var binder in Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetAllInstances(binderType))
          {
            var wrapper = (IBind<object, object>)Activator.CreateInstance(wrapperType, binder);
            wrapper.Bind(stepContext, queryResult.Value, query);
          }
        }
      }
    }

    public class BindWrapper<F, T> : IBind<object, object>
    {
      readonly IBind<F, T> _target;

      public BindWrapper(IBind<F, T> target)
      {
        _target = target;
      }

      public void Bind(StepContext stepContext, object from, object to)
      {
        _target.Bind(stepContext, (F)from, (T)to);
      }
    }
  }

  public interface IQueryBinder
  {
    IQuery Bind(IQuery query, StepContext stepContext);
    ICommand Bind(ICommand command, StepContext stepContext);
  }
}

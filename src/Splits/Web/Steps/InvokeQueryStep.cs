using System;
using System.Collections.Generic;
using Splits.Application;

namespace Splits.Web.Steps
{
  public class InvokeQueryStep : Step
  {
    public Type QueryType { get; private set; }
    public Type ResultType { get; private set; }
    public IStep ValidationErrorStep { get; private set; }
    public Action<IQuery, StepContext> Bind { get; private set; }
    public Func<StepContext, IQuery> CreateAndBind { get; private set; }
    public Func<StepContext, IDictionary<string, object>> CreateBindingDictionary { get; private set; }

    public InvokeQueryStep(Type queryType, Type resultType, Func<StepContext, IQuery> createAndBind)
    {
      QueryType = queryType;
      ResultType = resultType;
      CreateAndBind = createAndBind;
      Bind = (q, s) => { };
      CreateBindingDictionary = sc => new AggregateDictionary(sc.RequestContext);
    }

    public InvokeQueryStep(Type queryType, Type resultType, Action<IQuery, StepContext> bind)
    {
      QueryType = queryType;
      ResultType = resultType;
      Bind = bind;
      CreateBindingDictionary = sc => new AggregateDictionary(sc.RequestContext);
    }

    public InvokeQueryStep(Type queryType, Type resultType)
      : this(queryType, resultType, (q, s) => { })
    {
    }

    public InvokeQueryStep OnValidationError(IStep step)
    {
      ValidationErrorStep = step;
      return this;
    }

    public InvokeQueryStep BindingTo(Func<StepContext, IDictionary<string, object>> createBindingDictionary)
    {
      CreateBindingDictionary = createBindingDictionary;
      return this;
    }
  }
}
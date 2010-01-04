using System;
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

    public InvokeQueryStep(Type queryType, Type resultType, Func<StepContext, IQuery> createAndBind)
    {
      QueryType = queryType;
      ResultType = resultType;
      Bind = (q, s) => { };
      CreateAndBind = createAndBind;
    }

    public InvokeQueryStep(Type queryType, Type resultType, Action<IQuery, StepContext> bind)
    {
      QueryType = queryType;
      ResultType = resultType;
      Bind = bind;
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
  }
}
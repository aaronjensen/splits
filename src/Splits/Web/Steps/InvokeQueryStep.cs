using System;

namespace Splits.Web.Steps
{
  public class InvokeQueryStep : Step
  {
    public Type QueryType { get; private set; }
    public Type ResultType { get; private set; }
    public IStep ValidationErrorStep { get; private set; }
    public Action<object, StepContext> Bind { get; private set; }

    public InvokeQueryStep(Type queryType, Type resultType, Action<object, StepContext> bind)
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
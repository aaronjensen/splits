using System;

namespace Splits.Web.Steps
{
  public class InvokeQueryStep : Step
  {
    public Type QueryType { get; private set; }
    public Type ReplyType { get; private set; }
    public IStep ValidationErrorStep { get; private set; }
    public Func<StepContext, object> QueryFactory { get; private set; }

    public InvokeQueryStep(Type queryType, Type replyType)
    {
      QueryType = queryType;
      ReplyType = replyType;
    }

    public InvokeQueryStep Query(Func<StepContext, object> queryFactory)
    {
      QueryFactory = queryFactory;
      return this;
    }

    public InvokeQueryStep OnValidationError(IStep step)
    {
      ValidationErrorStep = step;
      return this;
    }
  }
}
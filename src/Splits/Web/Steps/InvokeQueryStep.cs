using System;

namespace Splits.Web.Steps
{
  public class InvokeQueryStep : Step
  {
    public Type QueryType { get; private set; }
    public Type ReplyType { get; private set; }
    public IStep ValidationErrorStep { get; private set; }

    public InvokeQueryStep(Type queryType, Type replyType)
    {
      QueryType = queryType;
      ReplyType = replyType;
    }

    public InvokeQueryStep OnValidationError(IStep step)
    {
      ValidationErrorStep = step;
      return this;
    }
  }
}
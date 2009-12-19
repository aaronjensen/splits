using System;

namespace Splits.Web.Steps
{
  public class InvokeCommandStep : Step
  {
    public Type CommandType { get; private set; }
    public Type ReplyType { get; private set; }
    public IStep SuccessStep { get; private set; }
    public IStep FailureStep { get; private set; }
    public IStep ValidationErrorStep { get; private set; }
    public IStep UsingQueryStep { get; private set; }

    public InvokeCommandStep(Type commandType, Type replyType)
    {
      CommandType = commandType;
      ReplyType = replyType;
    }

    public InvokeCommandStep OnSuccess(IStep step)
    {
      SuccessStep = step;
      return this;
    }

    public InvokeCommandStep OnFailure(IStep step)
    {
      FailureStep = step;
      return this;
    }

    public InvokeCommandStep OnValidationError(IStep step)
    {
      ValidationErrorStep = step;
      return this;
    }

    public InvokeCommandStep From(InvokeQueryStep step)
    {
      UsingQueryStep = step;
      return this;
    }
  }
}
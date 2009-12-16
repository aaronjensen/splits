using System;

namespace Splits.Web.Steps
{
  public class InvokeCommandStep : Step
  {
    readonly Type _commandType;

    public Type CommandType
    {
      get { return _commandType; }
    }

    public IStep SuccessStep { get; private set; }
    public IStep FailureStep { get; private set; }
    public IStep ValidationErrorStep { get; private set; }

    public InvokeCommandStep(Type commandType)
    {
      _commandType = commandType;
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
  }
}
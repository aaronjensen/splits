using System;
using System.Net;

namespace Splits.Web.Steps
{
  public class InvokeCommandStep : Step
  {
    public Type CommandType { get; private set; }
    public Type ResultType { get; private set; }
    public IStep SuccessStep { get; private set; }
    public IStep FailureStep { get; private set; }
    public IStep ValidationErrorStep { get; private set; }

    public InvokeCommandStep(Type commandType, Type resultType)
    {
      CommandType = commandType;
      ResultType = resultType;
      SuccessStep = new StatusStep(HttpStatusCode.OK);
      FailureStep = new StatusStep(HttpStatusCode.InternalServerError);
      ValidationErrorStep = new NoopStep();
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
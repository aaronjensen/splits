using System;
using System.Net;

namespace Splits.Web.Steps
{
  public class InvokeCommandStep : Step
  {
    public Type CommandType { get; private set; }
    public Type ResultType { get; private set; }
    public Action<object, StepContext> Bind { get; private set; }
    public Func<StepContext, object> CreateAndBind { get; private set; }
    public IStep SuccessStep { get; private set; }
    public IStep FailureStep { get; private set; }
    public IStep ValidationErrorStep { get; private set; }

    public InvokeCommandStep(Type commandType, Type resultType, Action<object, StepContext> bind)
    {
      CommandType = commandType;
      ResultType = resultType;
      SuccessStep = new StatusStep(HttpStatusCode.OK);
      FailureStep = new StatusStep(HttpStatusCode.InternalServerError);
      ValidationErrorStep = new NoopStep();
      Bind = bind;
    }

    public InvokeCommandStep(Type commandType, Type resultType, Func<StepContext, object> createAndBind)
    {
      CommandType = commandType;
      ResultType = resultType;
      SuccessStep = new StatusStep(HttpStatusCode.OK);
      FailureStep = new StatusStep(HttpStatusCode.InternalServerError);
      ValidationErrorStep = new NoopStep();
      CreateAndBind = createAndBind;
    }

    public InvokeCommandStep(Type commandType, Type resultType)
      : this(commandType, resultType, (c, s) => { })
    {
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
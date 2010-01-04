using System;
using System.Net;
using Splits.Application;

namespace Splits.Web.Steps
{
  public class InvokeCommandStep : Step
  {
    public Type CommandType { get; private set; }
    public Type ResultType { get; private set; }
    public Action<ICommand, StepContext> Bind { get; private set; }
    public Func<StepContext, ICommand> CreateAndBind { get; private set; }
    public IStep SuccessStep { get; private set; }
    public IStep FailureStep { get; private set; }
    public IStep ValidationErrorStep { get; private set; }

    public InvokeCommandStep(Type commandType, Type resultType, Action<ICommand, StepContext> bind)
    {
      CommandType = commandType;
      ResultType = resultType;
      SuccessStep = new StatusStep(HttpStatusCode.OK);
      FailureStep = new StatusStep(HttpStatusCode.InternalServerError);
      ValidationErrorStep = new NoopStep();
      Bind = bind;
    }

    public InvokeCommandStep(Type commandType, Type resultType, Func<StepContext, ICommand> createAndBind)
    {
      CommandType = commandType;
      ResultType = resultType;
      SuccessStep = new StatusStep(HttpStatusCode.OK);
      FailureStep = new StatusStep(HttpStatusCode.InternalServerError);
      ValidationErrorStep = new NoopStep();
      Bind = (q, s) => { };
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
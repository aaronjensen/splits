using System;
using System.Collections.Generic;
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
    public Func<StepContext, IDictionary<string, object>> CreateBindingDictionary { get; private set; }
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
      CreateBindingDictionary = sc => new AggregateDictionary(sc.RequestContext);
      Bind = bind;
    }

    public InvokeCommandStep(Type commandType, Type resultType, Func<StepContext, ICommand> createAndBind)
    {
      CommandType = commandType;
      ResultType = resultType;
      SuccessStep = new StatusStep(HttpStatusCode.OK);
      FailureStep = new StatusStep(HttpStatusCode.InternalServerError);
      ValidationErrorStep = new NoopStep();
      CreateAndBind = createAndBind;
      CreateBindingDictionary = sc => new AggregateDictionary(sc.RequestContext);
      Bind = (q, s) => { };
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

    public InvokeCommandStep BindingTo(Func<StepContext, IDictionary<string, object>> createBindingDictionary)
    {
      CreateBindingDictionary = createBindingDictionary;
      return this;
    }
  }
}
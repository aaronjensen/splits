using System;
using System.Collections.Generic;
using Splits.Application;
using Splits.Web.ModelBinding;
using Splits.Web.Steps;
using Splits.Web.Validation;

namespace Splits.Web.StepHandlers
{
  public class InvokeQueryStepHandler : IStepHandler<InvokeQueryStep>
  {
    readonly IQueryInvoker _queryInvoker;
    readonly IModelBinder _modelBinder;
    readonly IModelValidator _modelValidator;
    readonly IStepInvoker _stepInvoker;
    readonly IViewRenderer _viewRenderer;

    public InvokeQueryStepHandler(IQueryInvoker queryInvoker, IModelBinder modelBinder, IModelValidator modelValidator, IStepInvoker stepInvoker, IViewRenderer viewRenderer)
    {
      _queryInvoker = queryInvoker;
      _viewRenderer = viewRenderer;
      _modelBinder = modelBinder;
      _modelValidator = modelValidator;
      _stepInvoker = stepInvoker;
    }

    public Continuation Handle(InvokeQueryStep step, StepContext stepContext)
    {
      var querySpec = BindQuerySpec(step, stepContext);
      if (querySpec == null && step.ValidationErrorStep != null)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }
      
      var validationResult = _modelValidator.Validate(querySpec);
      if (!validationResult.IsValid && step.ValidationErrorStep != null)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }

      var query = _queryInvoker.Invoke(querySpec);
      stepContext.AddQuery(querySpec, query, step.ReplyType.Name);
      _viewRenderer.RenderModel(stepContext, query, step.ReplyType.Name);
      return Continuation.Continue;
    }

    IQuery BindQuerySpec(InvokeQueryStep step, StepContext stepContext)
    {
      if (step.QueryFactory != null)
      {
        return (IQuery)step.QueryFactory(stepContext);
      }
      var bindResult = _modelBinder.Bind(step.QueryType, new AggregateDictionary(stepContext.RequestContext));
      if (!bindResult.WasSuccessful)
      {
        return null;
      }
      return (IQuery)bindResult.Value;
    }
  }
}

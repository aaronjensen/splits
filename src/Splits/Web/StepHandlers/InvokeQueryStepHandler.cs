using System;
using System.Collections.Generic;

using Splits.Application;
using Splits.Internal;
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
    readonly IQueryBinder _queryBinder;
    readonly IStepInvoker _stepInvoker;

    public InvokeQueryStepHandler(IQueryInvoker queryInvoker, IModelBinder modelBinder, IModelValidator modelValidator, IQueryBinder queryBinder, IStepInvoker stepInvoker)
    {
      _queryInvoker = queryInvoker;
      _modelBinder = modelBinder;
      _modelValidator = modelValidator;
      _queryBinder = queryBinder;
      _stepInvoker = stepInvoker;
    }

    public Continuation Handle(InvokeQueryStep step, StepContext stepContext)
    {
      var query = step.CreateAndBind == null ? BindQuery(step, stepContext) : step.CreateAndBind(stepContext);
      if (query == null && step.ValidationErrorStep != null)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }

      if (query == null)
      {
        query = (IQuery)Activator.CreateInstance(step.QueryType);
      }

      query = BindToPreviousQueries(query, stepContext);
      step.Bind(query, stepContext);
      
      var validationResult = _modelValidator.Validate(query);
      if (!validationResult.IsValid && step.ValidationErrorStep != null)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }

      var queryResult = _queryInvoker.Invoke(query);
      stepContext.AddQuery(query, queryResult, step.ResultType.Name);
      return Continuation.Continue;
    }

    IQuery BindToPreviousQueries(IQuery query, StepContext stepContext)
    {
      return _queryBinder.Bind(query, stepContext);
    }

    IQuery BindQuery(InvokeQueryStep step, StepContext stepContext)
    {
      var bindResult = _modelBinder.Bind(step.QueryType, new AggregateDictionary(stepContext.RequestContext));
      if (!bindResult.WasSuccessful)
      {
        return null;
      }
      return (IQuery)bindResult.Value;
    }
  }
}

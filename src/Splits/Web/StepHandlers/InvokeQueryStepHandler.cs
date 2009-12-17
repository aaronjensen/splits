using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public InvokeQueryStepHandler(IQueryInvoker queryInvoker, IModelBinder modelBinder, IModelValidator modelValidator)
    {
      _queryInvoker = queryInvoker;
      _modelBinder = modelBinder;
      _modelValidator = modelValidator;
    }

    public Continuation Handle(InvokeQueryStep step, StepContext stepContext)
    {
      var bindResult = _modelBinder.Bind(step.QueryType,
        new AggregateDictionary(stepContext.RequestContext));

      if (!bindResult.WasSuccessful)
      {
        return Continuation.Abort;
      }

      var result = _queryInvoker.Invoke(bindResult.Value);

      stepContext.Response.Write(result.ToString());
      return Continuation.Continue;
    }
  }
}

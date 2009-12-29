using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Splits.Internal;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class RenderViewStepHandler : IStepHandler<RenderViewStep>
  {
    readonly IViewRenderer _viewRenderer;

    public RenderViewStepHandler(IViewRenderer viewRenderer)
    {
      _viewRenderer = viewRenderer;
    }

    public Continuation Handle(RenderViewStep step, StepContext stepContext)
    {
      var viewData = new ViewDataDictionary();
      var viewName = step.ViewName;
      if (String.IsNullOrEmpty(viewName) && step.ModelFactory == null)
      {
        var queryResult = stepContext.LastQueryResult;
        viewName = queryResult.GetType().Name;
        viewData.Model = queryResult;
      }
      if (step.ModelFactory != null)
      {
        viewData.Model = step.ModelFactory();
      }
      _viewRenderer.RenderViewData(stepContext, viewData, viewName);
      return Continuation.Stop;
    }
  }
}

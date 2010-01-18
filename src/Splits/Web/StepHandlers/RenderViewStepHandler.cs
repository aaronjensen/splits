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
      if (step.ModelFactory == null)
      {
        return Render(step.ViewName, stepContext.LastQueryOrCommandResult, step.SkipLayout, stepContext);
      }
      return Render(step.ViewName, step.ModelFactory(stepContext), step.SkipLayout, stepContext);
    }

    Continuation Render(string viewName, object model, bool skipLayout, StepContext stepContext)
    {
      var viewData = new ViewDataDictionary();
      viewData.Model = model;
      _viewRenderer.RenderViewData(stepContext, viewData, viewName ?? model.GetType().Name, skipLayout);
      return Continuation.Stop;
    }
  }
}

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
      if (step.ModelFactory != null)
      {
        viewData.Model = step.ModelFactory();
      }
      _viewRenderer.RenderViewData(stepContext, viewData, step.ViewName);
      return Continuation.Continue;
    }
  }
}

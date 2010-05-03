using System;
using System.Collections.Generic;

namespace Splits.Web.Steps
{
  public class RenderViewStep : Step
  {
    public Func<StepContext, object> ModelFactory { get; set; }
    public string ViewName { get; set; }
    public bool SkipLayout { get; set; }

    public RenderViewStep()
      : this(null)
    {
    }

    public RenderViewStep(string viewName)
    {
      ViewName = viewName;
    }

    public RenderViewStep(string viewName, Func<StepContext, object> modelFactory)
    {
      ViewName = viewName;
      ModelFactory = modelFactory;
    }
  }
}

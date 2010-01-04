using System;
using System.Collections.Generic;

namespace Splits.Web.Steps
{
  public class RenderViewStep : IStep
  {
    public Func<object> ModelFactory { get; set; }
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

    public RenderViewStep(string viewName, Func<object> modelFactory)
    {
      ViewName = viewName;
      ModelFactory = modelFactory;
    }

    public RenderViewStep(string viewName, object model)
      : this(viewName, () => model)
    {
    }
  }
}

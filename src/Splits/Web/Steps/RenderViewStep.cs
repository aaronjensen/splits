using System;
using System.Collections.Generic;

namespace Splits.Web.Steps
{
  public class RenderViewStep : IStep
  {
    public object Model { get; set; }
    public string ViewName { get; set; }

    public RenderViewStep(string viewName)
    {
      ViewName = viewName;
    }

    public RenderViewStep(string viewName, object model)
    {
      ViewName = viewName;
      Model = model;
    }
  }
}

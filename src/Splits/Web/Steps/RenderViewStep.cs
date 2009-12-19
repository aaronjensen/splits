using System;
using System.Collections.Generic;

namespace Splits.Web.Steps
{
  public class RenderViewStep : IStep
  {
    public string ViewName { get; set; }
    public string MasterViewName { get; set; }

    public RenderViewStep(string viewName)
    {
      ViewName = viewName;
    }

    public RenderViewStep(string viewName, string masterViewName)
    {
      ViewName = viewName;
      MasterViewName = masterViewName;
    }
  }
}

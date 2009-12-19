using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Splits.Web.Steps;
using SparkViewFactory = Splits.Web.Spark.SparkViewFactory;

namespace Splits.Web.StepHandlers
{
  public class RenderViewStepHandler : IStepHandler<RenderViewStep>
  {
    readonly static SparkViewFactory _factory = new SparkViewFactory();
    TempDataDictionary _tempData;
    ViewDataDictionary _viewData;

    public TempDataDictionary TempData
    {
      get
      {
        if (_tempData == null)
          _tempData = new TempDataDictionary();
        return _tempData;
      }
      set { _tempData = value; }
    }

    public ViewDataDictionary ViewData
    {
      get
      {
        if (_viewData == null)
          _viewData = new ViewDataDictionary();
        return _viewData;
      }
      set { _viewData = value; }
    }

    public bool SkipLayout { get; set; }
    public string[] ViewNames { get; set; }
    public string MasterName { get; set; }

    public Continuation Handle(RenderViewStep step, StepContext stepContext)
    {
      var view = _factory.FindView(stepContext.RequestContext, stepContext.UrlStrongPath, step.ViewName, String.Empty, true, true);
      if (view.View == null)
      {
        var locations = new StringBuilder();
        foreach (var location in view.SearchedLocations)
        {
          locations.AppendLine();
          locations.Append(location);
        }
        throw new InvalidOperationException("No view could be found in: " + locations);
      }

      var viewContext = new ViewContext(new ControllerContext(stepContext.RequestContext, new FakeControllerForControllerContext()), view.View, ViewData, TempData);
      view.View.Render(viewContext, stepContext.Response.Output);

      return Continuation.Continue;
    }

    class FakeControllerForControllerContext : Controller
    {
    }
  }
}

using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
using Splits.Web;
using SparkViewFactory = Splits.Web.Spark.SparkViewFactory;

namespace Splits.Internal
{
  public class ViewRenderer : IViewRenderer
  {
    readonly SparkViewFactory _factory;

    public ViewRenderer(SparkViewFactory factory)
    {
      _factory = factory;
    }

    public void RenderViewData(StepContext stepContext, ViewDataDictionary viewData, string viewName, bool skipLayout)
    {
      var view = _factory.FindView(stepContext.RequestContext, stepContext.UrlStrongPath, viewName, String.Empty, !skipLayout, true);
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

      stepContext.Fill(viewData);

      var controllerContext = new ControllerContext(stepContext.RequestContext, FakeController);
      var viewContext = new ViewContext(controllerContext, view.View, viewData, new TempDataDictionary(), stepContext.Response.Output);
      view.View.Render(viewContext, stepContext.Response.Output);
    }

    public void RenderModel(StepContext stepContext, object model, string viewName, bool skipLayout)
    {
      var viewData = new ViewDataDictionary();
      viewData.Model = model;
      RenderViewData(stepContext, viewData, viewName, skipLayout);
    }

    static readonly FakeControllerForControllerContext FakeController = new FakeControllerForControllerContext();
    class FakeControllerForControllerContext : Controller { }
  }

  public interface IViewRenderer
  {
    void RenderModel(StepContext stepContext, object model, string viewName, bool skipLayout);
    void RenderViewData(StepContext stepContext, ViewDataDictionary view, string viewName, bool skipLayout);
  }
}

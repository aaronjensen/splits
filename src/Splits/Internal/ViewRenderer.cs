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

    public void RenderViewData(StepContext stepContext, ViewDataDictionary viewData, string viewName)
    {
      var view = _factory.FindView(stepContext.RequestContext, stepContext.UrlStrongPath, viewName, String.Empty, true, true);
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
      var viewContext = new ViewContext(controllerContext, view.View, viewData, new TempDataDictionary());
      view.View.Render(viewContext, stepContext.Response.Output);
    }

    public void RenderModel(StepContext stepContext, object model, string viewName)
    {
      var viewData = new ViewDataDictionary();
      viewData.Model = model;
      RenderViewData(stepContext, viewData, viewName);
    }

    static readonly FakeControllerForControllerContext FakeController = new FakeControllerForControllerContext();
    class FakeControllerForControllerContext : Controller { }
  }

  public interface IViewRenderer
  {
    void RenderModel(StepContext stepContext, object model, string viewName);
    void RenderViewData(StepContext stepContext, ViewDataDictionary view, string viewName);
  }
}

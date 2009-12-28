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

  public interface IViewRenderer
  {
    void RenderModel(StepContext stepContext, object model, string viewName);
    void RenderViewData(StepContext stepContext, ViewDataDictionary view, string viewName);
  }

  public class ViewRenderer : IViewRenderer
  {
    readonly static SparkViewFactory _factory = new SparkViewFactory();

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

      var controllerContext = new ControllerContext(stepContext.RequestContext, FakeController);
      var viewContext = new ViewContext(controllerContext, view.View, viewData, new TempDataDictionary());
      view.View.Render(viewContext, stepContext.Response.Output);
    }

    public void RenderModel(StepContext stepContext, object model, string viewName)
    {
      var viewData = new ViewDataDictionary();
      viewData.Model = model;
      stepContext.Fill(viewData);
      RenderViewData(stepContext, viewData, viewName);
    }

    static readonly FakeControllerForControllerContext FakeController = new FakeControllerForControllerContext();
    class FakeControllerForControllerContext : Controller { }
  }
}

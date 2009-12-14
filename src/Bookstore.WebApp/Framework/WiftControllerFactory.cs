using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.ServiceLocation;

namespace Bookstore.WebApp.Framework
{
  public class WiftControllerFactory : IControllerFactory
  {
    readonly IControllerFactory _innerFactory;

    public WiftControllerFactory(IControllerFactory innerFactory)
    {
      _innerFactory = innerFactory;
    }

    public IController CreateController(RequestContext requestContext, string controllerName)
    {
      if (controllerName == "Wift")
      {
        return ServiceLocator.Current.GetInstance<IWiftController>();
      }

      return _innerFactory.CreateController(requestContext, controllerName);
    }

    public void ReleaseController(IController controller)
    {
      _innerFactory.ReleaseController(controller);
    }
  }

  public interface IWiftController : IController
  {
  }

  public class WiftController : IController
  {
    readonly IStepProvider _stepProvider;

    public WiftController(IStepProvider stepProvider)
    {
      _stepProvider = stepProvider;
    }

    public void Execute(RequestContext requestContext)
    {
      var urlType = requestContext.RouteData.DataTokens["urlType"] as Type;

      if (urlType == null) throw new InvalidOperationException("Can't get urlType!");

      IEnumerable<IStep> steps;
      if (String.Compare(requestContext.HttpContext.Request.HttpMethod, "GET", true) == 0)
      {
        steps = _stepProvider.GetStepsForGet(urlType);
      }
      else if (String.Compare(requestContext.HttpContext.Request.HttpMethod, "POST", true) == 0)
      {
        steps = _stepProvider.GetStepsForGet(urlType);
      }
      else
      {
        steps = new IStep[0];
      }

      var stepContext = new StepContext(requestContext);
      foreach (var step in steps)
      {
        if (step.ShouldApply(stepContext))
        {
          step.Apply(stepContext);
          if (step.Continuation != Continuation.Continue)
            break;
        }
      }
    }
  }
}

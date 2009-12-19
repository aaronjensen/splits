using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.ServiceLocation;

namespace Splits.Web
{
  public class SplitsController : IController
  {
    readonly IStepProvider _stepProvider;
    readonly IStepInvoker _stepInvoker;

    public SplitsController(IStepProvider stepProvider, IStepInvoker stepInvoker)
    {
      _stepProvider = stepProvider;
      _stepInvoker = stepInvoker;
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
        steps = _stepProvider.GetStepsForPost(urlType);
      }
      else
      {
        steps = new IStep[0];
      }

      var stepContext = new StepContext(requestContext, urlType);
      foreach (var step in steps)
      {
        var continuation = _stepInvoker.Invoke(step, stepContext);

        if (continuation != Continuation.Continue)
          break;
      }
    }
  }
}

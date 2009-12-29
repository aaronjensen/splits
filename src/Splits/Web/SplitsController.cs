using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using System.Web.Routing;

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

      if (!steps.Any())
      {
        HandleNoSteps(stepContext);
        return;
      }

      var lastContinuation = Continuation.Continue;
      foreach (var step in steps)
      {
        lastContinuation = _stepInvoker.Invoke(step, stepContext);
        if (lastContinuation != Continuation.Continue)
          break;
      }

      if (lastContinuation == Continuation.Continue)
      {
        HandleNoEndingStep(stepContext);
        return;
      }
    }

    static void HandleNoEndingStep(StepContext context)
    {
      context.Response.StatusCode = (Int32)HttpStatusCode.NoContent;
      context.Response.StatusDescription = "No Content";
    }

    static void HandleNoSteps(StepContext context)
    {
      context.Response.StatusCode = (Int32)HttpStatusCode.MethodNotAllowed;
      context.Response.StatusDescription = "Method Not Allowed";
    }
  }
}

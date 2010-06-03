using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using System.Web.Mvc;
using System.Web.Routing;

namespace Splits.Web
{
  public static class UrlStrongRoutingHelpers
  {
    public static bool IsUrlStrongRequest(this RequestContext requestContext)
    {
      return requestContext.RouteData.DataTokens.ContainsKey("urlType");
    }

    public static Type UrlStrongTypeFromRoute(this RequestContext requestContext)
    {
      var urlType = requestContext.RouteData.DataTokens["urlType"] as Type;
      if (urlType == null) throw new InvalidOperationException("Can't get urlType!");
      return urlType;
    }

    public static string UrlStrongPathFromRoute(this RequestContext requestContext)
    {
      var urlType = requestContext.UrlStrongTypeFromRoute();
      var parts = urlType.FullName.Split('+').Skip(1).Select(s => s.ToLower()).ToArray();
      return String.Join("/", parts);
    }
  }

  public class SplitsController : IController
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger("Splits");
    readonly IStepProvider _stepProvider;
    readonly IStepInvoker _stepInvoker;

    public SplitsController(IStepProvider stepProvider, IStepInvoker stepInvoker)
    {
      _stepProvider = stepProvider;
      _stepInvoker = stepInvoker;
    }

    IEnumerable<IStep> StepsFor(RequestContext requestContext, Type urlType)
    {
      var method = requestContext.HttpContext.Request.HttpMethod;
      if (String.Compare(method, "GET", true) == 0)
      {
        return _stepProvider.GetStepsForGet(urlType);
      }
      if (String.Compare(method, "POST", true) == 0)
      {
        return _stepProvider.GetStepsForPost(urlType);
      }
      return new IStep[0];
    }

    public void Execute(RequestContext requestContext)
    {
      var urlType = requestContext.UrlStrongTypeFromRoute();
      using (log4net.NDC.Push(urlType.FullName))
      {
        var steps = StepsFor(requestContext, urlType);
        var stepContext = new StepContext(requestContext, urlType);

        if (!steps.Any())
        {
          HandleNoSteps(stepContext);
          return;
        }

        var lastContinuation = Continuation.Continue;
        foreach (var step in steps)
        {
          _log.Info("Step: " + step);
          lastContinuation = _stepInvoker.Invoke(step, stepContext);
          if (lastContinuation != Continuation.Continue)
          {
            break;
          }
        }

        if (lastContinuation == Continuation.Continue)
        {
          HandleNoEndingStep(stepContext);
          return;
        }

        _log.Info("Completed");
      }
    }

    static void HandleNoEndingStep(StepContext context)
    {
      _log.Info("No Ending Step");
      context.Response.StatusCode = (Int32)HttpStatusCode.NoContent;
      context.Response.StatusDescription = "No Content";
    }

    static void HandleNoSteps(StepContext context)
    {
      _log.Info("No Steps");
      context.Response.StatusCode = (Int32)HttpStatusCode.MethodNotAllowed;
      context.Response.StatusDescription = "Method Not Allowed";
    }
  }
}

using System;
using System.Net;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class SeeOtherStepHandler : IStepHandler<SeeOtherStep>
  {
    public Continuation Handle(SeeOtherStep step, StepContext stepContext)
    {
      stepContext.Response.Redirect(step.GetUrl(stepContext).ToString(), false);
      stepContext.Response.StatusCode = (int)HttpStatusCode.SeeOther;
      stepContext.Response.StatusDescription = "See Other";

      return Continuation.RenderNow;
    }
  }
}
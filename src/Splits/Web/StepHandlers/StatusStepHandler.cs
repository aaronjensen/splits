using System;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class StatusStepHandler : IStepHandler<StatusStep>
  {
    public Continuation Handle(StatusStep step, StepContext stepContext)
    {
      stepContext.Response.StatusCode = step.Code;
      if (!String.IsNullOrEmpty(step.Message))
      {
        stepContext.Response.StatusDescription = step.Message;
      }
      return Continuation.Stop;
    }
  }
}
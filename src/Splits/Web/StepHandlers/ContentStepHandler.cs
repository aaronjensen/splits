using System;
using System.Collections.Generic;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class ContentStepHandler : IStepHandler<ContentStep>
  {
    public Continuation Handle(ContentStep step, StepContext stepContext)
    {
      stepContext.Response.Write(step.OutputFactory());

      return Continuation.Continue;
    }
  }
}

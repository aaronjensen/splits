using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class RenderStepHandler : IStepHandler<RenderStep>
  {
    public Continuation Handle(RenderStep step, StepContext stepContext)
    {
      stepContext.Response.Write(step.Text);

      return Continuation.Continue;
    }
  }
}

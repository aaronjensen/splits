using System;
using System.Collections.Generic;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class NoopStepHandler : IStepHandler<NoopStep>
  {
    public Continuation Handle(NoopStep step, StepContext stepContext)
    {
      return Continuation.Continue;
    }
  }
}

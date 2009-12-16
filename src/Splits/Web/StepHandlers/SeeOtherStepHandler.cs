using System;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class SeeOtherStepHandler : IStepHandler<SeeOtherStep>
  {
    public Continuation Handle(SeeOtherStep step, StepContext stepContext)
    {
      throw new NotImplementedException();
      return Continuation.RenderNow;
    }
  }
}
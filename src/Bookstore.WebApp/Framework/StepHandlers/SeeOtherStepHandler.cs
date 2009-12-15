using System;
using Bookstore.WebApp.Framework.Steps;

namespace Bookstore.WebApp.Framework.StepHandlers
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
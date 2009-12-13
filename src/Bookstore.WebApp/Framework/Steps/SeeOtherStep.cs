using System;
using Machine.UrlStrong;

namespace Bookstore.WebApp.Framework.Steps
{
  public class SeeOtherStep : UnconditionalStep
  {
    public SeeOtherStep(Func<StepContext, ISupportGet> getUrl)
    {
    }

    public override void Apply(StepContext stepContext)
    {
    }

    public override Continuation Continuation
    {
      get { return Continuation.RenderNow; }
    }
  }
}
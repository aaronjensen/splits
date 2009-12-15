using Bookstore.WebApp.Framework.Steps;

namespace Bookstore.WebApp.Framework.StepHandlers
{
  public class RedirectStepHandler : IStepHandler<RedirectStep>
  {
    public Continuation Handle(RedirectStep step, StepContext stepContext)
    {
      stepContext.Response.Redirect(step.GetUrl(stepContext).ToString());
      return Continuation.RenderNow;
    }
  }
}
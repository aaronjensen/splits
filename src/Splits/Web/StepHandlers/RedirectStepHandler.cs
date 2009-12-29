using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class RedirectStepHandler : IStepHandler<RedirectStep>
  {
    public Continuation Handle(RedirectStep step, StepContext stepContext)
    {
      stepContext.Response.Redirect(step.GetUrl(stepContext));
      return Continuation.Stop;
    }
  }
}
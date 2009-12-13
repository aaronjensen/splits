namespace Bookstore.WebApp.Framework.Steps
{
  public class NegatedConditionalStep : ConditionalStep
  {
    public NegatedConditionalStep(IStep step) : base(step)
    {
    }

    public override bool ShouldApply(StepContext stepContext)
    {
      return !base.ShouldApply(stepContext);
    }
  }
}
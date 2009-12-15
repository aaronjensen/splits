namespace Bookstore.WebApp.Framework.Steps
{
  public class NegatedConditionalStep : ConditionalStep
  {
    public NegatedConditionalStep(IStep step) : base(step)
    {
    }

    public override bool ConditionIsSatisfied(StepContext stepContext)
    {
      return !base.ConditionIsSatisfied(stepContext);
    }
  }
}
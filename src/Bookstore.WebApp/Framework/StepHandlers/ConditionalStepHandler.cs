using Bookstore.WebApp.Framework.Steps;

namespace Bookstore.WebApp.Framework.StepHandlers
{
  public class ConditionalStepHandler : IStepHandler<ConditionalStep>,
                                        IStepHandler<NegatedConditionalStep>
  {
    readonly IStepInvoker _stepInvoker;

    public ConditionalStepHandler(IStepInvoker stepInvoker)
    {
      _stepInvoker = stepInvoker;
    }

    public Continuation Handle(ConditionalStep step, StepContext stepContext)
    {
      if (step.ConditionIsSatisfied(stepContext))
      {
        return _stepInvoker.Invoke(step.InnerStep, stepContext);
      }

      return Continuation.Continue;
    }

    public Continuation Handle(NegatedConditionalStep step, StepContext stepContext)
    {
      return Handle((ConditionalStep)step, stepContext);
    }
  }
}
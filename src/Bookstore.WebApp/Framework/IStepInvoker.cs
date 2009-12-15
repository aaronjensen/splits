using System;

namespace Bookstore.WebApp.Framework
{
  public interface IStepInvoker
  {
    Continuation Invoke(IStep step, StepContext stepContext);
  }

  public class StepInvoker : IStepInvoker
  {
    readonly IStepHandlerLocator _locator;

    public StepInvoker(IStepHandlerLocator locator)
    {
      _locator = locator;
    }

    public Continuation Invoke(IStep step, StepContext stepContext)
    {
      if (step == null) throw new ArgumentNullException("step");
      if (stepContext == null) throw new ArgumentNullException("stepContext");

      var handler = _locator.GetStepHandler(step.GetType());

      if (handler == null) throw new ArgumentException("No handler for Step: " + step.GetType(), "step");

      return handler.Handle(step, stepContext);
    }
  }
}
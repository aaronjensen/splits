namespace Bookstore.WebApp.Framework.Steps
{
  public class InvokeCommandStep<T> : UnconditionalStep
  {
    public override void Apply(StepContext stepContext)
    {
    }

    public override Continuation Continuation
    {
      get { return Continuation.Continue; }
    }

    public InvokeCommandStep<T> OnSuccess(IStep step)
    {
      return this;
    }

    public InvokeCommandStep<T> OnFailure(IStep step)
    {
      return this;
    }

    public InvokeCommandStep<T> OnValidationError(IStep step)
    {
      return this;
    }
  }
}
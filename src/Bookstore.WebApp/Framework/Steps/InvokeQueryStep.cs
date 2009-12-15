namespace Bookstore.WebApp.Framework.Steps
{
  public class InvokeQueryStep<T> : UnconditionalStep
  {
    public override void Apply(StepContext stepContext)
    {
    }

    public override Continuation Continuation
    {
      get { return Continuation.Continue; }
    }
  }
}
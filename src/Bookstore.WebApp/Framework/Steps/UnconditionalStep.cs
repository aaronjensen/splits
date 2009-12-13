namespace Bookstore.WebApp.Framework.Steps
{
  public abstract class UnconditionalStep : IStep
  {
    public ConditionalStep Unless 
    { 
      get
      {
        return new NegatedConditionalStep(this);
      }
    }
    public ConditionalStep If 
    { 
      get
      {
        return new ConditionalStep(this);
      }
    }

    public bool ShouldApply(StepContext stepContext)
    {
      return true;
    }

    public abstract void Apply(StepContext stepContext);

    public bool IsValid()
    {
      return true;
    }

    public abstract Continuation Continuation
    { 
      get;
    }
  }
}
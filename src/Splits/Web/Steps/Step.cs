namespace Splits.Web.Steps
{
  public abstract class Step : IStep
  {
    public IConditionalStep Unless 
    { 
      get { return new NegatedConditionalStep(this); }
    }

    public IConditionalStep If 
    { 
      get { return new ConditionalStep(this); }
    }
  }
}
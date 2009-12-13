namespace Bookstore.WebApp.Framework
{
  public interface IStep
  {
    bool ShouldApply(StepContext stepContext);
    void Apply(StepContext stepContext);
    bool IsValid();
    Continuation Continuation { get; }
  }
}
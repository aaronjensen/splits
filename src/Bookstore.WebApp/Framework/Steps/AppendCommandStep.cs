using System;
using Bookstore.Application.Commands;

namespace Bookstore.WebApp.Framework.Steps
{
  public class AppendCommandStep<T> : UnconditionalStep
  {
    Func<StepContext, BookStoreSetupCommand> _createDefault;

    public override void Apply(StepContext stepContext)
    {
    }

    public override Continuation Continuation
    {
      get { return Continuation.Continue; }
    }

    public AppendCommandStep<T> DefaultTo(Func<StepContext, BookStoreSetupCommand> createDefault)
    {
      _createDefault = createDefault;

      return this;
    }
  }
}
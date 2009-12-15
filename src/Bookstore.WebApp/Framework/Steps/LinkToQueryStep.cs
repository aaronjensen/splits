using System;
using Bookstore.Application.Commands;

namespace Bookstore.WebApp.Framework.Steps
{
  public class LinkToQueryStep<T> : UnconditionalStep
  {
    Func<StepContext, T> _createDefault;

    public override void Apply(StepContext stepContext)
    {
      stepContext.Response.Write(typeof(T));
    }

    public override Continuation Continuation
    {
      get { return Continuation.Continue; }
    }

    public LinkToQueryStep<T> DefaultTo(Func<StepContext, T> createDefault)
    {
      _createDefault = createDefault;

      return this;
    }
  }
}
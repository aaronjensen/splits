using System;
using Machine.UrlStrong;

namespace Bookstore.WebApp.Framework.Steps
{
  public class RedirectStep : UnconditionalStep
  {
    readonly Func<StepContext, ISupportGet> _getUrl;

    public RedirectStep(Func<StepContext, ISupportGet> getUrl)
    {
      _getUrl = getUrl;
    }

    public override void Apply(StepContext stepContext)
    {
      stepContext.Response.Redirect(_getUrl(stepContext).ToString());
    }

    public override Continuation Continuation
    {
      get { return Continuation.RenderNow; }
    }
  }
}
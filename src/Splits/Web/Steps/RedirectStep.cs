using System;
using Machine.UrlStrong;

namespace Splits.Web.Steps
{
  public class RedirectStep : Step
  {
    readonly Func<StepContext, string> _getUrl;

    public string GetUrl(StepContext stepContext)
    {
      return _getUrl(stepContext);
    }

    public RedirectStep(Func<StepContext, string> getUrl)
    {
      _getUrl = getUrl;
    }
  }
}
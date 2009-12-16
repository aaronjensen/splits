using System;
using Machine.UrlStrong;

namespace Splits.Web.Steps
{
  public class RedirectStep : Step
  {
    readonly Func<StepContext, ISupportGet> _getUrl;

    public ISupportGet GetUrl(StepContext stepContext)
    {
      return _getUrl(stepContext);
    }

    public RedirectStep(Func<StepContext, ISupportGet> getUrl)
    {
      _getUrl = getUrl;
    }
  }
}
using System;
using Machine.UrlStrong;
using Splits.Web.Steps;

namespace Splits.Web
{
  public struct StepBuilder
  {
  }

  public static class StepBuilderExtensions
  {
    public static RedirectStep Redirect(this StepBuilder steps, Func<StepContext, ISupportGet> getUrl)
    {
      return new RedirectStep(getUrl);
    }

    public static LinkToCommandStep LinkToCommand<T>(this StepBuilder steps) 
    {
      return new LinkToCommandStep(typeof(T));
    }

    public static LinkToQueryStep LinkToQuery<T>(this StepBuilder steps)
    {
      return new LinkToQueryStep(typeof(T));
    }

    public static InvokeCommandStep InvokeCommand<T>(this StepBuilder steps)
    {
      return new InvokeCommandStep(typeof(T));
    }

    public static InvokeQueryStep InvokeQuery<T>(this StepBuilder steps)
    {
      return new InvokeQueryStep(typeof(T));
    }

    public static SeeOtherStep SeeOther(this StepBuilder steps, Func<StepContext, ISupportGet> getUrl)
    {
      return new SeeOtherStep(getUrl);
    }

    public static ContentStep Content(this StepBuilder steps, string text)
    {
      return new ContentStep(text);
    }

    public static RenderViewStep Render(this StepBuilder steps, string view)
    {
      return new RenderViewStep();
    }
  }
}
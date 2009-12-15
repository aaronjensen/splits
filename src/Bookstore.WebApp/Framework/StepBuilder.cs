using System;
using Bookstore.Application.Framework;
using Bookstore.WebApp.Framework.Steps;
using Machine.UrlStrong;

namespace Bookstore.WebApp.Framework
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

    public static InvokeQueryStep<T> InvokeQuery<T>(this StepBuilder steps)
    {
      return new InvokeQueryStep<T>();
    }

    public static SeeOtherStep SeeOther(this StepBuilder steps, Func<StepContext, ISupportGet> getUrl)
    {
      return new SeeOtherStep(getUrl);
    }
  }
}
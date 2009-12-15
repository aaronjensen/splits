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

    public static LinkToCommandStep<T> LinkToCommand<T>(this StepBuilder steps) 
    {
      return new LinkToCommandStep<T>();
    }

    public static LinkToQueryStep<T> LinkToQuery<T>(this StepBuilder steps)
    {
      return new LinkToQueryStep<T>();
    }

    public static InvokeCommandStep<T> InvokeCommand<T>(this StepBuilder steps)
    {
      return new InvokeCommandStep<T>();
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
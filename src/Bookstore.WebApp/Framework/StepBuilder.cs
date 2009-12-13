using System;
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

    public static AppendCommandStep<T> AppendCommand<T>(this StepBuilder steps)
    {
      return new AppendCommandStep<T>();
    }

    public static InvokeCommandStep<T> InvokeCommand<T>(this StepBuilder steps)
    {
      return new InvokeCommandStep<T>();
    }

    public static SeeOtherStep SeeOther(this StepBuilder steps, Func<StepContext, ISupportGet> getUrl)
    {
      return new SeeOtherStep(getUrl);
    }
  }
}
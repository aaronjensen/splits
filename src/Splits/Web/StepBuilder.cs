using System;
using System.Web;

using Machine.UrlStrong;

using Splits.Application;
using Splits.Queries;
using Splits.Web.Steps;
using Splits.Internal;

namespace Splits.Web
{
  public struct StepBuilder
  {
    public StepBuilder Require { get { return this; } }
  }

  public static class StepBuilderExtensions
  {
    public static IStep Authentication(this StepBuilder steps)
    {
      return new StatusStep(0x191).Unless.True(sc => new IsAuthenticatedQuerySpec());
    }

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
      var replyType = typeof(T).GetGenericArgumentInImplementationOf(typeof(ICommand<>));
      if (replyType == null) throw new ArgumentException("T should be an ICommand<>");
      return new InvokeCommandStep(typeof(T), replyType);
    }

    public static InvokeQueryStep InvokeQuery<T>(this StepBuilder steps)
    {
      var replyType = typeof(T).GetGenericArgumentInImplementationOf(typeof(IQuery<>));
      if (replyType == null) throw new ArgumentException("T should be an IQuery<>");
      return new InvokeQueryStep(typeof(T), replyType);
    }

    public static SeeOtherStep SeeOther(this StepBuilder steps, Func<StepContext, ISupportGet> getUrl)
    {
      return new SeeOtherStep(getUrl);
    }

    public static ContentStep Content(this StepBuilder steps, Func<string> textFactory)
    {
      return new ContentStep(textFactory);
    }

    public static ContentStep Content(this StepBuilder steps, string text)
    {
      return new ContentStep(() => text);
    }

    public static RenderViewStep Render(this StepBuilder steps, string view)
    {
      return new RenderViewStep(view);
    }

    public static RenderViewStep Render<T>(this StepBuilder steps, T model, string view)
    {
      return new RenderViewStep(view, model);
    }

    public static RenderViewStep Render<T>(this StepBuilder steps, T model)
    {
      return steps.Render(model, typeof(T).Name);
    }
  }
}
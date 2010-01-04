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

    public static RedirectStep Redirect(this StepBuilder steps, Func<StepContext, string> getUrl)
    {
      return new RedirectStep(getUrl);
    }

    public static RedirectStep Redirect(this StepBuilder steps, Func<StepContext, ISupportGet> getUrl)
    {
      return steps.Redirect(sc => getUrl(sc).ToString());
    }

    public static RedirectStep RedirectToReferrer(this StepBuilder steps)
    {
      return steps.Redirect(sc => sc.Request.UrlReferrer.ToString());
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
      return steps.InvokeCommand<T>((q, c) => { });
    }

    public static InvokeCommandStep InvokeCommand<T>(this StepBuilder steps, Func<StepContext, T> createAndBind) where T : ICommand
    {
      var resultType = typeof(T).GetGenericArgumentInImplementationOf(typeof(ICommand<>));
      if (resultType == null) throw new ArgumentException("T should be an ICommand<>");
      return new InvokeCommandStep(typeof(T), resultType, (s) => createAndBind(s));
    }

    public static InvokeCommandStep InvokeCommand<T>(this StepBuilder steps, Action<T, StepContext> bind)
    {
      var resultType = typeof(T).GetGenericArgumentInImplementationOf(typeof(ICommand<>));
      if (resultType == null) throw new ArgumentException("T should be an ICommand<>");
      return new InvokeCommandStep(typeof(T), resultType, (c, s) => bind((T)c, s));
    }

    public static InvokeQueryStep InvokeQuery<T>(this StepBuilder steps)
    {
      return steps.InvokeQuery<T>((q, c) => { });
    }

    public static InvokeQueryStep InvokeQuery<T>(this StepBuilder steps, Func<StepContext, T> createAndBind) where T : IQuery
    {
      var resultType = typeof(T).GetGenericArgumentInImplementationOf(typeof(IQuery<>));
      if (resultType == null) throw new ArgumentException("T should be an IQuery<>");
      return new InvokeQueryStep(typeof(T), resultType, (s) => createAndBind(s));
    }

    public static InvokeQueryStep InvokeQuery<T>(this StepBuilder steps, Action<T, StepContext> bind)
    {
      var resultType = typeof(T).GetGenericArgumentInImplementationOf(typeof(IQuery<>));
      if (resultType == null) throw new ArgumentException("T should be an IQuery<>");
      return new InvokeQueryStep(typeof(T), resultType, (q, s) => bind((T)q, s));
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

    public static RenderViewStep Render(this StepBuilder steps)
    {
      return new RenderViewStep();
    }

    public static RenderViewStep Render(this StepBuilder steps, string view)
    {
      return new RenderViewStep(view);
    }

    public static RenderViewStep Render<T>(this StepBuilder steps, string view, Func<StepContext, T> model)
    {
      return new RenderViewStep(view, s => model(s));
    }

    public static RenderViewStep Render<T>(this StepBuilder steps, Func<StepContext, T> model)
    {
      return steps.Render(null, model);
    }

    public static RenderViewStep RenderNoLayout(this StepBuilder steps)
    {
      return new RenderViewStep { SkipLayout = true };
    }
  }
}
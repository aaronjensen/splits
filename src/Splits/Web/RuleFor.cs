using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Machine.UrlStrong;

namespace Splits.Web
{
  public abstract class RuleFor<TUrl> : IRule where TUrl : IUrl
  {
    public bool ShouldApply(Type urlType)
    {
      return urlType == typeof(TUrl);
    }

    public virtual IEnumerable<IStep> OnAny(Type urlType)
    {
      yield break;
    }

    public virtual IEnumerable<IStep> OnGet(Type urlType)
    {
      yield break;
    }

    public virtual IEnumerable<IStep> OnPost(Type urlType)
    {
      yield break;
    }

    public virtual IEnumerable<IStep> OnPut(Type urlType)
    {
      yield break;
    }

    public virtual IEnumerable<IStep> OnDelete(Type urlType)
    {
      yield break;
    }

    public IEnumerable<IStep> OnHead(Type urlType)
    {
      yield break;
    }

    protected StepBuilder Steps;
  }
}
using System;
using System.Collections.Generic;
using Machine.UrlStrong;

namespace Splits.Web
{
  public interface IRule
  {
    bool ShouldApply(Type urlType);
    IEnumerable<IStep> OnAny(Type urlType);
    IEnumerable<IStep> OnGet(Type urlType);
    IEnumerable<IStep> OnPost(Type urlType);
    IEnumerable<IStep> OnPut(Type urlType);
    IEnumerable<IStep> OnDelete(Type urlType);
  }

  public class RuleFor<TUrl> : IRule where TUrl : IUrl
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

    protected StepBuilder Steps;
  }
}
using System;
using System.Collections.Generic;

namespace Splits.Web
{
  public abstract class GlobalRule : IRule
  {
    public bool ShouldApply(Type urlType)
    {
      return true;
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
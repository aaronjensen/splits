using System;
using System.Collections.Generic;

namespace Splits.Web
{
  public abstract class Rule : IRule
  {
    public abstract bool ShouldApply(Type urlType);

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
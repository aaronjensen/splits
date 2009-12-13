using System;
using System.Collections.Generic;

namespace Bookstore.WebApp.Framework
{
  public class GlobalRule : IRule
  {
    public virtual bool ShouldApply(Type urlType)
    {
      return true;
    }

    public virtual IEnumerable<IStep> OnAny(Type url)
    {
      yield break;
    }

    public virtual IEnumerable<IStep> OnGet(Type url)
    {
      yield break;
    }

    public virtual IEnumerable<IStep> OnPost(Type url)
    {
      yield break;
    }

    public virtual IEnumerable<IStep> OnPut(Type url)
    {
      yield break;
    }

    public virtual IEnumerable<IStep> OnDelete(Type url)
    {
      yield break;
    }

    protected StepBuilder Steps;
  }
}
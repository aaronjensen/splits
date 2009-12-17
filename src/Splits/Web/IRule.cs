using System;
using System.Collections.Generic;

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
    IEnumerable<IStep> OnHead(Type urlType);
  }
}
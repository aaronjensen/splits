using System;
using System.Collections.Generic;
using Bookstore.Application.Queries;
using Bookstore.WebApp;
using Splits.Web;

namespace Bookstore.WebApp.Rules
{
  public class RequireSetupRule
    : Rule
  {
    public override bool ShouldApply(Type urlType)
    {
      return false;
      return urlType != typeof(Urls.Root.Bookstore_setup);
    }

    public override IEnumerable<IStep> OnAny(Type urlType)
    {
      yield return Steps.Redirect(c => Urls.root.bookstore_setup).Unless.True(c => new IsBookStoreSetup());
    }
  }
}
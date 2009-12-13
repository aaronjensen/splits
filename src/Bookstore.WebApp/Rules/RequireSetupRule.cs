using System;
using System.Collections.Generic;
using Bookstore.Application.Queries;
using Bookstore.WebApp.Framework;

namespace Bookstore.WebApp.Rules
{
  public class RequireSetupRule
    : GlobalRule
  {
    public override IEnumerable<IStep> OnAny(Type url)
    {
      if (url == typeof(Urls.Root.Bookstore_setup)) yield break;

      yield return Steps.Redirect(c => Urls.root.bookstore_setup).Unless.True(c => new IsBookStoreSetup());
    }
  }
}
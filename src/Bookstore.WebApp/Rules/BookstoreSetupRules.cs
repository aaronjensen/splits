using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bookstore.Application.Commands;
using Bookstore.Application.Queries;
using Bookstore.WebApp.Framework;

namespace Bookstore.WebApp.Rules
{
  public class BookstoreSetupRules : 
    RuleFor<Urls.Root.Bookstore_setup>
  {
    public override IEnumerable<IStep> OnGet(Type url)
    {
      yield return Steps.Redirect(c => Urls.root).If.True(c => new IsBookStoreSetup());
      yield return Steps.AppendCommand<BookStoreSetupCommand>().DefaultTo(c => new BookStoreSetupCommand {Name = "My Bookstore"});
    }

    public override IEnumerable<IStep> OnPost(Type url)
    {
      yield return Steps.InvokeCommand<BookStoreSetupCommand>()
        .OnSuccess(Steps.SeeOther(c => Urls.root))
        .OnFailure(Steps.Redirect(c => Urls.root.bookstore_setup))
        .OnValidationError(Steps.Redirect(c => Urls.root.bookstore_setup));
    }
  }
}
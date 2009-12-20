using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Commands;
using Bookstore.Application.Queries;
using Splits.Web;

namespace Bookstore.WebApp.Rules
{
  public class SeeOtherRules : RuleFor<Urls.Root.See_other>
  {
    public override IEnumerable<IStep>  OnGet(Type urlType)
    {
      yield return Steps.SeeOther(c => Urls.root.other);
    }
  }

  public class OtherRules : RuleFor<Urls.Root.Other>
  {
    public override IEnumerable<IStep>  OnGet(Type urlType)
    {
      yield return Steps.Content("Other!");
    }
  }

  public class TestRules : RuleFor<Urls.Root.Test>
  {
    public override IEnumerable<IStep> OnGet(Type urlType)
    {
      yield return Steps.InvokeQuery<TestQuery>();
      yield return Steps.InvokeCommand<TestCommand>()
        .OnSuccess(Steps.Render("success"))
        .OnFailure(Steps.Render("failure"))
        .OnValidationError(Steps.Render("validation error"));
    }
  }
}

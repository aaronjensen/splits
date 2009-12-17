using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Commands;
using Bookstore.Application.Queries;
using Splits.Web;

namespace Bookstore.WebApp.Rules
{
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

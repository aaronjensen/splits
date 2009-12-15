using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.WebApp.Framework.Steps;

namespace Bookstore.WebApp.Framework.StepHandlers
{
  public class LinkToCommandStepHandler : IStepHandler<LinkToCommandStep>
  {
    public Continuation Handle(LinkToCommandStep step, StepContext stepContext)
    {
      stepContext.Response.Write("Command : " + step.CommandType);

      return Continuation.Continue;
    }
  }
}

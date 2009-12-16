using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splits.Web.ModelBinding;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class LinkToCommandStepHandler : IStepHandler<LinkToCommandStep>
  {
    readonly IModelBinder _modelBinder;

    public LinkToCommandStepHandler(IModelBinder modelBinder)
    {
      _modelBinder = modelBinder;
    }

    public Continuation Handle(LinkToCommandStep step, StepContext stepContext)
    {
      //_modelBinder.Bind(step.CommandType, stepContext.Request.Params);
      stepContext.Response.Write("Command : " + step.CommandType);

      return Continuation.Continue;
    }
  }
}

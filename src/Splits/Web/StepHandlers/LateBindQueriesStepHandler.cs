using System;
using System.Collections.Generic;
using Splits.Internal;
using Splits.Web.Steps;

namespace Splits.Web.StepHandlers
{
  public class LateBindQueriesStepHandler : IStepHandler<LateBindQueriesStep>
  {
    readonly IQueryBinder _queryBinder;

    public LateBindQueriesStepHandler(IQueryBinder queryBinder)
    {
      _queryBinder = queryBinder;
    }

    public Continuation Handle(LateBindQueriesStep step, StepContext stepContext)
    {
      return Continuation.Continue;
    }
  }
}

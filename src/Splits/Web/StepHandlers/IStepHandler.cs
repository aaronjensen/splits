using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Splits.Web.StepHandlers
{
  public interface IStepHandler<TStep> where TStep : IStep
  {
    Continuation Handle(TStep step, StepContext stepContext);
  }
}
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Bookstore.WebApp.Framework.StepHandlers
{
  public interface IStepHandler<TStep> where TStep : IStep
  {
    Continuation Handle(TStep step, StepContext stepContext);
  }
}
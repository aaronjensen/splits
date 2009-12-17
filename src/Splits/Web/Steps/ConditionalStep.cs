using System;
using Microsoft.Practices.ServiceLocation;
using Splits.Application;

namespace Splits.Web.Steps
{
  public interface IConditionalStep
  {
    ConditionalStep True(Func<StepContext, IQuery<bool>> queryFunc);
  }

  public class ConditionalStep : IStep, IConditionalStep
  {
    readonly IStep _innerStep;
    Func<StepContext, bool> _condition;

    public IStep InnerStep
    {
      get { return _innerStep; }
    }

    public virtual bool ConditionIsSatisfied(StepContext stepContext)
    {
      return _condition(stepContext);
    }

    public ConditionalStep(IStep step)
    {
      if (step == null) throw new ArgumentNullException("step");
      _condition = c => true;
      _innerStep = step;
    }

    public ConditionalStep True(Func<StepContext, IQuery<bool>> queryFunc)
    {
      var oldCondition = _condition;
      var queryInvoker = ServiceLocator.Current.GetInstance<IQueryInvoker>();

      _condition = c => oldCondition(c) && (bool)queryInvoker.Invoke(queryFunc(c));

      return this;
    }
  }
}
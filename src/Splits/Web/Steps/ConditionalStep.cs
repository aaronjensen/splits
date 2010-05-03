using System;
using Microsoft.Practices.ServiceLocation;
using Splits.Application;
using System.Web.Mvc;

namespace Splits.Web.Steps
{
  public interface IConditionalStep
  {
    ConditionalStep Ajax();
    ConditionalStep True(Func<StepContext, IQuery<bool>> queryFunc);
    ConditionalStep True(Func<StepContext, bool> queryFunc);
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

    public ConditionalStep Ajax()
    {
      var oldCondition = _condition;
      _condition = c => oldCondition(c) && c.Request.IsAjaxRequest();
      return this;
    }

    public ConditionalStep True(Func<StepContext, IQuery<bool>> queryFunc)
    {
      var oldCondition = _condition;
      var queryInvoker = ServiceLocator.Current.GetInstance<IQueryInvoker>();
      _condition = c => oldCondition(c) && (bool)queryInvoker.Invoke(queryFunc(c));
      return this;
    }

    public ConditionalStep True(Func<StepContext, bool> queryFunc)
    {
      var oldCondition = _condition;
      _condition = c => oldCondition(c) && queryFunc(c);
      return this;
    }
  }
}
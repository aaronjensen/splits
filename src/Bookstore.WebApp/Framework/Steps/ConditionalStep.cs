using System;
using Bookstore.Application;
using Bookstore.Application.Framework;
using Microsoft.Practices.ServiceLocation;

namespace Bookstore.WebApp.Framework.Steps
{
  public class ConditionalStep : IStep
  {
    readonly IStep _step;
    bool _isValid;
    Func<StepContext, bool> _condition;

    public ConditionalStep(IStep step)
    {
      if (step == null) throw new ArgumentNullException("step");
      _condition = c => true;
      _step = step;
    }

    public ConditionalStep True(Func<StepContext, IQuery<bool>> queryFunc)
    {
      _isValid = true;
      var oldCondition = _condition;
      var querier = ServiceLocator.Current.GetInstance<IQuerier>();

      _condition = c => oldCondition(c) && querier.Get(queryFunc(c));

      return this;
    }

    public virtual bool ShouldApply(StepContext stepContext)
    {
      if (!IsValid()) throw new InvalidOperationException("This isn't a valid step, it's missing its condition");
      return _condition(stepContext) &&  _step.ShouldApply(stepContext);
    }

    public void Apply(StepContext stepContext)
    {
      _step.Apply(stepContext);
    }

    public bool IsValid()
    {
      return _isValid;
    }

    public Continuation Continuation
    {
      get { return _step.Continuation; }
    }
  }
}
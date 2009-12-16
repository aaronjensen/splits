using System;
using Microsoft.Practices.ServiceLocation;
using Splits.Web.StepHandlers;

namespace Splits.Web
{
  public interface IStepHandlerLocator
  {
    IStepHandler<IStep> GetStepHandler(Type stepType);
  }

  public class StepHandlerLocator : IStepHandlerLocator
  {
    class Wrapper : IStepHandler<IStep>
    {
      object _handler;

      public Wrapper(object handler)
      {
        _handler = handler;
      }

      public Continuation Handle(IStep step, StepContext stepContext)
      {
        var method = _handler.GetType().GetMethod("Handle", new Type[] {step.GetType(), typeof(StepContext)} );

        var continuation = method.Invoke(_handler, new object[] { step, stepContext });

        return (Continuation)continuation;
      }
    }

    public IStepHandler<IStep> GetStepHandler(Type stepType)
    {
      if (stepType == null) throw new ArgumentNullException("stepType");

      var handlerType = typeof(IStepHandler<>).MakeGenericType(stepType);

      var handler = ServiceLocator.Current.GetInstance(handlerType);

      return new Wrapper(handler);
    }
  }
}
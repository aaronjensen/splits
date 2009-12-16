using Splits.Application;
using Splits.Application.Impl;
using Splits.Web.ModelBinding;
using Splits.Web.Steps;
using Splits.Web.Validation;

namespace Splits.Web.StepHandlers
{
  public class InvokeCommandStepHandler : IStepHandler<InvokeCommandStep>
  {
    readonly ICommandInvoker _commandInvoker;
    readonly IModelBinder _modelBinder;
    readonly IModelValidator _modelValidator;
    readonly IStepInvoker _stepInvoker;

    public InvokeCommandStepHandler(ICommandInvoker commandInvoker, IModelBinder modelBinder, IModelValidator modelValidator, IStepInvoker stepInvoker)
    {
      _commandInvoker = commandInvoker;
      _modelBinder = modelBinder;
      _modelValidator = modelValidator;
      _stepInvoker = stepInvoker;
    }

    public Continuation Handle(InvokeCommandStep step, StepContext stepContext)
    {
      var bindResult = _modelBinder.Bind(step.CommandType,
                                         new AggregateDictionary(stepContext.RequestContext));

      if (!bindResult.WasSuccessful)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }

      var result = _commandInvoker.Invoke(bindResult.Value);
      if (result.WasSuccessful)
      {
        return _stepInvoker.Invoke(step.SuccessStep, stepContext);
      }

      return _stepInvoker.Invoke(step.FailureStep, stepContext);
    }
  }
}

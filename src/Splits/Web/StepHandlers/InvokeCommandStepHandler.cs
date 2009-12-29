using Splits.Application;
using Splits.Internal;
using Splits.Web.ModelBinding;
using Splits.Web.Steps;
using Splits.Web.Validation;

namespace Splits.Web.StepHandlers
{
  public class InvokeCommandStepHandler : IStepHandler<InvokeCommandStep>
  {
    readonly IQueryBinder _queryBinder;
    readonly ICommandInvoker _commandInvoker;
    readonly IModelBinder _modelBinder;
    readonly IModelValidator _modelValidator;
    readonly IStepInvoker _stepInvoker;

    public InvokeCommandStepHandler(ICommandInvoker commandInvoker, IModelBinder modelBinder, IModelValidator modelValidator, IStepInvoker stepInvoker, IQueryBinder queryBinder)
    {
      _commandInvoker = commandInvoker;
      _queryBinder = queryBinder;
      _modelBinder = modelBinder;
      _modelValidator = modelValidator;
      _stepInvoker = stepInvoker;
    }

    public Continuation Handle(InvokeCommandStep step, StepContext stepContext)
    {
      var bindResult = _modelBinder.Bind(step.CommandType, new AggregateDictionary(stepContext.RequestContext));
      if (!bindResult.WasSuccessful)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }

      var validationResult = _modelValidator.Validate(bindResult.Value);
      if (!validationResult.IsValid)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }

      var result = _commandInvoker.Invoke(BindToPreviousQueries((ICommand)bindResult.Value, stepContext));
      if (result.WasSuccessful)
      {
        return _stepInvoker.Invoke(step.SuccessStep, stepContext);
      }

      return _stepInvoker.Invoke(step.FailureStep, stepContext);
    }

    ICommand BindToPreviousQueries(ICommand command, StepContext stepContext)
    {
      return _queryBinder.Bind(command, stepContext);
    }
  }
}

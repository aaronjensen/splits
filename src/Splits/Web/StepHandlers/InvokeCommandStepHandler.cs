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
      var command = step.CreateAndBind == null ? BindCommand(step, stepContext) : step.CreateAndBind(stepContext);
      if (command == null && step.ValidationErrorStep != null)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }

      command = BindToPreviousQueries(command, stepContext);
      step.Bind(command, stepContext);

      var validationResult = _modelValidator.Validate(command);
      if (!validationResult.IsValid)
      {
        return _stepInvoker.Invoke(step.ValidationErrorStep, stepContext);
      }

      var result = _commandInvoker.Invoke(command).Result;
      stepContext.AddCommand(command, result, step.ResultType.Name);
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

    ICommand BindCommand(InvokeCommandStep step, StepContext stepContext)
    {
      var bindResult = _modelBinder.Bind(step.CommandType, step.CreateBindingDictionary(stepContext));
      if (!bindResult.WasSuccessful)
      {
        return null;
      }
      return (ICommand)bindResult.Value;
    }
  }
}

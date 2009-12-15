using Bookstore.Application;
using Bookstore.WebApp.Framework.Steps;

namespace Bookstore.WebApp.Framework.StepHandlers
{
  public class InvokeCommandStepHandler : IStepHandler<InvokeCommandStep>
  {
    readonly ICommandInvoker _commandInvoker;

    public InvokeCommandStepHandler(ICommandInvoker commandInvoker, IModelBinder modelBinder)
    {
      _commandInvoker = commandInvoker;
    }

    public Continuation Handle(InvokeCommandStep step, StepContext stepContext)
    {
      return Continuation.Continue;
    }
  }
}
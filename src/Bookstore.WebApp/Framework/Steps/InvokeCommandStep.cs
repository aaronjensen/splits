using System;

namespace Bookstore.WebApp.Framework.Steps
{
  public class InvokeCommandStep : Step
  {
    readonly Type _commandType;

    public Type CommandType
    {
      get { return _commandType; }
    }

    public InvokeCommandStep(Type commandType)
    {
      _commandType = commandType;
    }

    public InvokeCommandStep OnSuccess(IStep step)
    {
      return this;
    }

    public InvokeCommandStep OnFailure(IStep step)
    {
      return this;
    }

    public InvokeCommandStep OnValidationError(IStep step)
    {
      return this;
    }
  }
}
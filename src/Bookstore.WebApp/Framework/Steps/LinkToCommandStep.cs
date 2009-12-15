using System;
using Bookstore.Application.Commands;

namespace Bookstore.WebApp.Framework.Steps
{
  public class LinkToCommandStep : Step
  {
    readonly Type _commandType;
    Func<StepContext, object> _createDefault;

    public Type CommandType
    {
      get { return _commandType; }
    }

    public LinkToCommandStep(Type commandType)
    {
      _commandType = commandType;
    }

    public object CreateDefault(StepContext stepContext)
    {
      return _createDefault(stepContext);
    }

    public LinkToCommandStep DefaultTo(Func<StepContext, object> createDefault)
    {
      _createDefault = createDefault;

      return this;
    }
  }
}
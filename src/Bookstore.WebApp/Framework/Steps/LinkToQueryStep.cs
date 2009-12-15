using System;
using Bookstore.Application.Commands;

namespace Bookstore.WebApp.Framework.Steps
{
  public class LinkToQueryStep : Step
  {
    readonly Type _queryType;
    Func<StepContext, object> _createDefault;

    public Type QueryType
    {
      get { return _queryType; }
    }

    public LinkToQueryStep(Type queryType)
    {
      _queryType = queryType;
    }

    public object CreateDefault(StepContext stepContext)
    {
      return _createDefault(stepContext);
    }

    public LinkToQueryStep DefaultTo(Func<StepContext, object> createDefault)
    {
      _createDefault = createDefault;

      return this;
    }
  }
}
using System;

namespace Splits.Web.Steps
{
  public class InvokeQueryStep : Step
  {
    readonly Type _queryType;

    public Type QueryType
    {
      get { return _queryType; }
    }

    public InvokeQueryStep(Type queryType)
    {
      _queryType = queryType;
    }
  }
}
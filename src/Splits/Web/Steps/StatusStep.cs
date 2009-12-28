using System;

namespace Splits.Web.Steps
{
  public class StatusStep : Step
  {
    readonly Int32 _code;
    readonly string _message;

    public Int32 Code
    {
      get { return _code; }
    }

    public string Message
    {
      get { return _message; }
    }

    public StatusStep(Int32 code)
      : this(code, null)
    {
    }

    public StatusStep(Int32 code, string message)
    {
      _code = code;
      _message = message;
    }
  }
}
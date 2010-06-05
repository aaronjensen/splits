using System;
using System.Collections.Generic;

namespace Splits.Web
{
  public class ValidationResult
  {
    public static readonly ValidationResult Success;
    readonly IEnumerable<string> _memberNames;
    string _errorMessage;

    protected ValidationResult(ValidationResult validationResult)
    {
      if (validationResult == null) throw new ArgumentNullException("validationResult");
      _errorMessage = validationResult._errorMessage;
      _memberNames = validationResult._memberNames;
    }

    public ValidationResult(string errorMessage)
      : this(errorMessage, null)
    {
    }

    public ValidationResult(string errorMessage, IEnumerable<string> memberNames)
    {
      _errorMessage = errorMessage;
      _memberNames = memberNames ?? new string[0];
    }

    public string ErrorMessage
    {
      get { return _errorMessage; }
      set { _errorMessage = value; }
    }

    public IEnumerable<string> MemberNames
    {
      get { return _memberNames;
      }
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splits.Web.Validation
{
  public class ValidationResult
  {
    public bool IsValid { get; private set; }

    public ValidationResult(bool isValid)
    {
      IsValid = isValid;
    }
  }

  public interface IModelValidator
  {
    ValidationResult Validate(object model);
  }

  public class PollyannaValidator : IModelValidator
  {
    public ValidationResult Validate(object model)
    {
      return new ValidationResult(true);
    }
  }
}

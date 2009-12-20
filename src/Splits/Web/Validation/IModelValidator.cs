using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Splits.Web.Validation
{
  public class ValidationResult
  {
    readonly List<ValidationError> _errors;
    public bool IsValid { get; private set; }
    public IEnumerable<ValidationError> Errors { get { return _errors; } }

    public ValidationResult()
    {
      _errors = new List<ValidationError>();
      IsValid = true;
    }

    public void AddError(string error)
    {
      IsValid = false;
      _errors.Add(new ValidationError {ErrorMessage = error});
    }
  }

  public class ValidationError
  {
    public string ErrorMessage { get; set; }
  }

  public interface IModelValidator
  {
    ValidationResult Validate(object model);
  }

  public class ModelValidator : IModelValidator
  {
    public ValidationResult Validate(object model)
    {
      var result = new ValidationResult();
      foreach (var property in model.GetType().GetProperties())
      {
        foreach (var attribute in property.GetCustomAttributes(typeof(ValidationAttribute), true).Cast<ValidationAttribute>())
        {
          var value = property.GetValue(model, null);
          if (!attribute.IsValid(value))
          {
            result.AddError(attribute.FormatErrorMessage(property.Name));
          }
        }
      }

      return result;
    }
  }

  public class PollyannaValidator : IModelValidator
  {
    public ValidationResult Validate(object model)
    {
      return new ValidationResult();
    }
  }
}

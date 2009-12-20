using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Splits.Web.Validation
{
  public class ValidatorResult
  {
    readonly List<ValidationError> _errors;
    public bool IsValid { get; private set; }
    public IEnumerable<ValidationError> Errors { get { return _errors; } }

    public ValidatorResult()
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
    ValidatorResult Validate(object model);
  }

  public class ModelValidator : IModelValidator
  {
    readonly IServiceProvider _serviceProvider;

    public ModelValidator(IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public ValidatorResult Validate(object model)
    {
      var result = new ValidatorResult();
      var results = new List<ValidationResult>();
      var context = new ValidationContext(model, _serviceProvider, null);

      Validator.TryValidateObject(model, context, results, true);

      foreach (var error in results.Select(x => x.ErrorMessage))
      {
        result.AddError(error);
      }

      return result;
    }
  }

  public class PollyannaValidator : IModelValidator
  {
    public ValidatorResult Validate(object model)
    {
      return new ValidatorResult();
    }
  }

  public class ValidateAttribute : ValidationAttribute
  {
    public override bool IsValid(object value)
    {
      var context = new ValidationContext(value, null, null);

      return Validator.TryValidateObject(value, context, null, true);
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      if (value == null) return ValidationResult.Success;

      var newContext = new ValidationContext(value, validationContext, validationContext.Items);

      var results = new List<ValidationResult>();
      Validator.TryValidateObject(value, newContext, results, true);
      if (results.Any())
        return results.First();

      return ValidationResult.Success;
    }
  }
}

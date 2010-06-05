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

    public void AddError(string error, IEnumerable<string> memberNames)
    {
      IsValid = false;
      _errors.Add(new ValidationError(error, memberNames));
    }
  }

  public class ValidationError
  {
    public string ErrorMessage { get; private set; }
    public IEnumerable<string> MemberNames { get; private set; }

    public ValidationError(string errorMessage, IEnumerable<string> memberNames)
    {
      ErrorMessage = errorMessage;
      MemberNames = memberNames;
    }
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

      // Validator.TryValidateObject(model, results, true);

      foreach (var error in results)
      {
        result.AddError(error.ErrorMessage, error.MemberNames);
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
      //var context = new ValidationContext(value, null, null);

      //return Validator.TryValidateObject(value, context, null, true);
      return true;
    }
  }
}

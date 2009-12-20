using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Machine.Specifications;
using Splits.Web.Validation;

namespace Splits.Specs.ModelValidation
{
  [Subject(typeof(ModelValidator))]
  public class when_validating_a_required_int : ModelValidatorSpecs
  {
    Because of = () => 
      result = validator.Validate(new RequiredInt());

    It should_always_be_valid = () =>
      result.IsValid.ShouldBeTrue();
  }

  [Subject(typeof(ModelValidator))]
  public class when_validating_a_required_nullable_int_that_is_null : ModelValidatorSpecs
  {
    Because of = () => 
      result = validator.Validate(new RequiredNullableInt());

    It should_not_be_valid = () =>
      result.IsValid.ShouldBeFalse();
  }

  [Subject(typeof(ModelValidator))]
  public class when_validating_a_required_nullable_int_that_is_not_null : ModelValidatorSpecs
  {
    Because of = () => 
      result = validator.Validate(new RequiredNullableInt() { RequiredId = 3 });

    It should_be_valid = () =>
      result.IsValid.ShouldBeTrue();
  }

  public class ModelValidatorSpecs
  {
    Establish context = () =>
    {
      validator = new ModelValidator();
    };

    protected static ModelValidator validator;
    protected static ValidationResult result;
  }

  public class RequiredInt
  {
    [Required]
    public int RequiredId { get; set; }
  }

  public class RequiredNullableInt
  {
    [Required]
    public int? RequiredId { get; set; }
  }
}
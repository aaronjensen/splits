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

  [Subject(typeof(ModelValidator))]
  public class when_validating_valid_nested_classes : ModelValidatorSpecs
  {
    Because of = () => 
      result = validator.Validate(new Parent() { Child = new Child { Id = 3} });

    It should_be_valid = () =>
      result.IsValid.ShouldBeTrue();
  }

  [Subject(typeof(ModelValidator))]
  public class when_validating_invalid_nested_classes : ModelValidatorSpecs
  {
    Because of = () => 
      result = validator.Validate(new Parent() { Child = new Child { Id = null} });

    It should_not_be_valid = () =>
      result.IsValid.ShouldBeFalse();

    It should_have_the_full_nested_property_name = () =>
      result.Errors.First().MemberNames.First().ShouldEqual("Child.Id");
  }

  [Subject(typeof(ModelValidator))]
  public class when_validating_nested_classes_with_missing_required_child : ModelValidatorSpecs
  {
    Because of = () => 
      result = validator.Validate(new Parent() { Child = null });

    It should_not_be_valid = () =>
      result.IsValid.ShouldBeFalse();
  }

  [Subject(typeof(ModelValidator))]
  public class when_validating_nested_classes_with_invalid_grand_child : ModelValidatorSpecs
  {
    Because of = () => 
      result = validator.Validate(new Parent() { Child = new Child() { Id = 3, GrandChild = new GrandChild { Foo = 10 }} });

    It should_not_be_valid = () =>
      result.IsValid.ShouldBeFalse();

    It should_have_the_full_nested_property_name = () =>
      result.Errors.First().MemberNames.First().ShouldEqual("Child.GrandChild.Foo");
  }

  public class ModelValidatorSpecs
  {
    Establish context = () =>
    {
      validator = new ModelValidator(null);
    };

    protected static ModelValidator validator;
    protected static ValidatorResult result;
  }

  public class Parent 
  {
    [Validate]
    [Required]
    public Child Child { get; set; }
  }

  public class Child 
  {
    [Required]
    public int? Id { get; set; }
    [Validate]
    public GrandChild GrandChild { get; set;}
  }

  public class GrandChild
  {
    [Range(0, 3)]
    public int Foo { get; set; }
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
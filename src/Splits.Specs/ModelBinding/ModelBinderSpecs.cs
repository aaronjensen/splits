using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using Machine.Specifications;
using Microsoft.Practices.ServiceLocation;
using Rhino.Mocks;
using Splits.Web;
using Splits.Web.ModelBinding;
using Splits.Web.ModelBinding.DefaultConverterFamilies;

namespace Splits.Specs.ModelBinding
{
  [Subject("Model Binder")]
  public class when_binding_a_simple_query : ModelBinderSpecs
  {
    Establish context = () =>
    {
      dictionary["id"] = "3";
    };

    Because of = () => result = binder.Bind(typeof(SimpleQuery), dictionary);

    It should_bind_the_property = () =>
      ((SimpleQuery)result.Value).Id.ShouldEqual(3);
  }

  [Subject("Model Binder")]
  public class when_binding_a_simple_query_with_a_prefix : ModelBinderSpecs
  {
    Establish context = () =>
    {
      dictionary["prefix-id"] = "3";
    };

    Because of = () => result = binder.Bind(typeof(SimpleQuery), dictionary, "prefix");

    It should_bind_the_property = () =>
      ((SimpleQuery)result.Value).Id.ShouldEqual(3);
  }

  [Subject("ModelBinder")]
  public class when_binding_a_child_object : ModelBinderSpecs
  {
    Establish context = () =>
    {
      dictionary["id"] = "3";
      dictionary["child-id"] = "4";
    };

    Because of = () => result = binder.Bind(typeof(Parent), dictionary);

    It should_bind_the_parent_property = () =>
      ((Parent)result.Value).Id.ShouldEqual(3);

    It should_bind_the_child_property = () =>
      ((Parent)result.Value).Child.Id.ShouldEqual(4);
  }

  public class ModelBinderSpecs
  {
    Establish context = () =>
    {
      var container = MockRepository.GenerateStub<IServiceLocator>();
      ServiceLocator.SetLocatorProvider(() => container);
      container.Stub(x => x.GetAllInstances<IConverterFamily>()).Return(new IConverterFamily[] { new NullableFamily() });
      binder = new StandardModelBinder(new TypeDescriptorRegistry(), new ValueConverterRegistry());
      dictionary = new RouteValueDictionary();
    };

    protected static StandardModelBinder binder;
    protected static IDictionary<string, object> dictionary;
    protected static BindResult result;
  }

  public class SimpleQuery
  {
    public int Id { get; set; }
  }

  public class Parent
  {
    public int Id { get; set; }
    public Child Child { get; set; }
  }

  public class Child
  {
    public int Id { get; set; }
  }
}
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

namespace Splits.Specs
{
  [Subject("Model Binder")]
  public class when_binding_a_simple_query
  {
    Establish context = () =>
    {
      var container = MockRepository.GenerateStub<IServiceLocator>();
      ServiceLocator.SetLocatorProvider(() => container);
      container.Stub(x=>x.GetAllInstances<IConverterFamily>()).Return(new IConverterFamily[0]);
      binder = new StandardModelBinder(new TypeDescriptorRegistry(), new ValueConverterRegistry());
      dictionary = new RouteValueDictionary();
      dictionary["id"] = "3";
    };

    Because of = () => result = binder.Bind(typeof(SimpleQuery), dictionary);

    It should_bind_the_property = () =>
      ((SimpleQuery)result.Value).Id.ShouldEqual(3);

    static IDictionary<string, object> dictionary;
    static IModelBinder binder;
    static BindResult result;
  }

  [Subject("Model Binder")]
  public class when_binding_a_simple_query_with_a_prefix
  {
    Establish context = () =>
    {
      var container = MockRepository.GenerateStub<IServiceLocator>();
      ServiceLocator.SetLocatorProvider(() => container);
      container.Stub(x=>x.GetAllInstances<IConverterFamily>()).Return(new IConverterFamily[0]);
      binder = new StandardModelBinder(new TypeDescriptorRegistry(), new ValueConverterRegistry());
      dictionary = new RouteValueDictionary();
      dictionary["prefixId"] = "3";
    };

    Because of = () => result = binder.Bind(typeof(SimpleQuery), dictionary, "prefix");

    It should_bind_the_property = () =>
      ((SimpleQuery)result.Value).Id.ShouldEqual(3);

    static IDictionary<string, object> dictionary;
    static IModelBinder binder;
    static BindResult result;
  }

  public class SimpleQuery
  {
    public int Id { get; set; }
  }
}
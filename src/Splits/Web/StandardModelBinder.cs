using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Splits.Web
{
  public class StandardModelBinder : IModelBinder
  {
    readonly ITypeDescriptorRegistry _typeDescriptorRegistry;

    public StandardModelBinder(ITypeDescriptorRegistry typeDescriptorRegistry)
    {
      _typeDescriptorRegistry = typeDescriptorRegistry;
    }

    public bool Matches(Type type)
    {
      return type.GetConstructors().Count(x => x.GetParameters().Length == 0) == 1;
    }

    public BindResult Bind(Type type, IDictionary<string, object> data)
    {
      var result = new BindResult();

      result.Value = Activator.CreateInstance(type);

      Populate(result, type, data);

      return result;
    }

    private void Populate(BindResult result, Type type, IDictionary<string, object> data)
    {
      _typeDescriptorRegistry.ForEachProperty(type,
                                              prop => SetPropertyValue(prop, data[prop.Name], result));
    }

    private void SetPropertyValue(PropertyInfo property, object rawValue, BindResult result)
    {
      try
      {
        object value = ConvertValue(property, rawValue);
        property.SetValue(result.Value, value, null);
      }
      catch (Exception e)
      {
        var problem = new ConvertProblem
        {
          Exception = e,
          Item = result.Value,
          Property = property,
          Value = rawValue
        };

        result.Problems.Add(problem);
      }
    }

    public object ConvertValue(PropertyInfo property, object rawValue)
    {
      return rawValue;
      /*_converters[property.PropertyType](new RawValue
        {
            Locator = locator,
            Property = property,
            Value = rawValue
        });*/
    }
  }
}
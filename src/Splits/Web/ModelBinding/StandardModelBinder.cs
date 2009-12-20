using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Splits.Web.ModelBinding
{
  public class StandardModelBinder : IModelBinder
  {
    readonly ITypeDescriptorRegistry _typeDescriptorRegistry;
    readonly IValueConverterRegistry _converters;

    public StandardModelBinder(ITypeDescriptorRegistry typeDescriptorRegistry, IValueConverterRegistry converters)
    {
      _typeDescriptorRegistry = typeDescriptorRegistry;
      _converters = converters;
    }

    public bool Matches(Type type)
    {
      return type.GetConstructors().Count(x => x.GetParameters().Length == 0) == 1;
    }

    public BindResult Bind(Type type, IDictionary<string, object> data)
    {
      return Bind(type, data, "");
    }

    public BindResult Bind(Type type, IDictionary<string, object> data, string prefix)
    {
      if (prefix == null) throw new ArgumentNullException("prefix");

      var result = new BindResult();

      result.Value = Activator.CreateInstance(type);

      Populate(result, type, data, prefix);

      return result;
    }
    
    private string AddPrefix(string prefix, string name)
    {
      if (string.IsNullOrEmpty(prefix))
        return name;

      return prefix + "-" + name;
    }

    private void Populate(BindResult result, Type type, IDictionary<string, object> data, string prefix) {
      _typeDescriptorRegistry.ForEachProperty(type,
                                              prop => SetPropertyValue(prop, data[AddPrefix(prefix, prop.Name)], result));
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
      return _converters[property.PropertyType](new RawValue
      {
        Property = property,
        Value = rawValue
      });
    }
  }
}
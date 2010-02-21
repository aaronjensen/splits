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

      result.Value = Create(result, type, data, prefix);
      Populate(result, type, data, prefix);
      return result;
    }

    static string AddPrefix(string prefix, string name)
    {
      if (string.IsNullOrEmpty(prefix))
        return name;
      return prefix + "." + name;
    }

    object Create(BindResult result, Type type, IDictionary<string, object> data, string prefix)
    {
      var selected = _typeDescriptorRegistry.SelectConstructor(type, ctor => {
        var hasValues = ctor.GetParameters().Aggregate(true, (a, b) => a && data.ContainsKey(AddPrefix(prefix, b.Name)));
        return hasValues ? ctor : null;
      });
      if (selected == null)
      {
        return Activator.CreateInstance(type);
      }
      var parameters = new List<object>();
      foreach (var parameter in selected.GetParameters())
      {
        var rawValue = data[AddPrefix(prefix, parameter.Name)];
        try
        {
          var converted = ConvertValue(parameter, rawValue, data, prefix);
          parameters.Add(converted);
        }
        catch (Exception e)
        {
          var problem = new ConvertProblem
          {
            Exception = e,
            Item = result.Value,
            Parameter = parameter,
            Value = rawValue
          };

          result.Problems.Add(problem);
        }
      }
      return selected.Invoke(parameters.ToArray());
    }

    void Populate(BindResult result, Type type, IDictionary<string, object> data, string prefix)
    {
      _typeDescriptorRegistry.ForEachWritableProperty(type, prop => SetPropertyValue(prop, data[AddPrefix(prefix, prop.Name)], result, data, prefix));
    }

    void SetPropertyValue(PropertyInfo property, object rawValue, BindResult result, IDictionary<string, object> data, string prefix)
    {
      try
      {
        var value = ConvertValue(property, rawValue, data, prefix);
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

    public object ConvertValue(ParameterInfo parameter, object rawValue, IDictionary<string, object> data, string prefix)
    {
      return ConvertValue(parameter.ParameterType, parameter.Name, rawValue, data, prefix);
    }

    public object ConvertValue(PropertyInfo property, object rawValue, IDictionary<string, object> data, string prefix)
    {
      return ConvertValue(property.PropertyType, property.Name, rawValue, data, prefix);
    }

    public object ConvertValue(Type type, string name, object rawValue, IDictionary<string, object> data, string prefix)
    {
      var converter = _converters[type];

      if (converter == null)
      {
        return Bind(type, data, AddPrefix(prefix, name)).Value;
      }

      return converter(new RawValue {
        TargetName = name,
        Value = rawValue
      });
    }
  }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.ServiceLocation;

namespace Splits.Web
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

    private void Populate(BindResult result, Type type, IDictionary<string, object> data, string prefix)
    {
      _typeDescriptorRegistry.ForEachProperty(type,
                                              prop => SetPropertyValue(prop, data[prefix + prop.Name], result));
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

  public class BooleanFamily : IConverterFamily
  {
    public bool Matches(Type type)
    {
      return type == typeof(bool);
    }

    public ValueConverter Build(IValueConverterRegistry registry, Type type)
    {
      return x =>
      {
        if (x.Value.ToString().Contains(x.Property.Name)) return true;

        return ValueConverterRegistry.BasicConvert(typeof(bool), x.Value);
      };
    }
  }

  public class ValueConverterRegistry : IValueConverterRegistry
  {
    private readonly Cache<Type, ValueConverter> _converters;
    private readonly List<IConverterFamily> _families = new List<IConverterFamily>();

    public ValueConverterRegistry(IEnumerable<IConverterFamily> families)
    {
      _families.AddRange(families);

      _converters =
          new Cache<Type, ValueConverter>(t => { return _families.Find(x => x.Matches(t)).Build(this, t); });

      AddPolicies();
    }

    public ValueConverter this[Type type] { get { return _converters[type]; } }

    private void AddPolicies()
    {
      If(t => true).Use((r, type) => x => BasicConvert(type, x.Value));
    }

    public void AddFamily(IConverterFamily family)
    {
      _families.Insert(0, family);
    }

    public ConverterExpression If(Predicate<Type> matches)
    {
      return new ConverterExpression(matches, f => _families.Add(f));
    }

    public void Add<T>() where T : IConverterFamily, new()
    {
      _families.Add(new T());
    }

    public static object BasicConvert(Type type, object original)
    {
      if (type.IsAssignableFrom(original.GetType())) return original;

      return TypeDescriptor.GetConverter(type).ConvertFrom(original);
    }
  }

  public interface IConverterFamily
  {
    bool Matches(Type type);
    ValueConverter Build(IValueConverterRegistry registry, Type type);
  }

  public class ConverterExpression
  {
    private readonly Predicate<Type> _matches;
    private readonly Action<ConverterFamily> _register;

    public ConverterExpression(Predicate<Type> matches, Action<ConverterFamily> register)
    {
      _matches = matches;
      _register = register;
    }

    public void Use(Func<IValueConverterRegistry, Type, ValueConverter> builder)
    {
      _register(new ConverterFamily(_matches, builder));
    }
  }

  public class ConverterFamily : IConverterFamily
  {
    private readonly Func<IValueConverterRegistry, Type, ValueConverter> _builder;
    private readonly Predicate<Type> _matches;

    public ConverterFamily(Predicate<Type> matches, Func<IValueConverterRegistry, Type, ValueConverter> builder)
    {
      _matches = matches;
      _builder = builder;
    }


    public bool Matches(Type type)
    {
      return _matches(type);
    }

    public ValueConverter Build(IValueConverterRegistry registry, Type type)
    {
      return _builder(registry, type);
    }
  }

  public class RawValue
  {
    public PropertyInfo Property;
    public object Value;
  }

  public delegate object ValueConverter(RawValue value);

  public interface IValueConverterRegistry
  {
    ValueConverter this[Type type] { get; }
  }

  public class NullableFamily : IConverterFamily
  {
    public bool Matches(Type type)
    {
      return type.IsNullableOfT();
    }

    public ValueConverter Build(IValueConverterRegistry registry, Type type)
    {
      Type innerType = type.GetGenericArguments()[0];
      ValueConverter inner = registry[innerType];

      return x =>
      {
        if (x.Value == null || (x.Value is string && (string)x.Value == string.Empty)) return null;

        return inner(x);
      };
    }
  }
}
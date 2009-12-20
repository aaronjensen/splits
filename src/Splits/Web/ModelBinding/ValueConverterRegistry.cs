using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Splits.Web.ModelBinding
{
  public class ValueConverterRegistry : IValueConverterRegistry
  {
    private readonly Cache<Type, ValueConverter> _converters;
    private readonly List<IConverterFamily> _families = new List<IConverterFamily>();

    public ValueConverterRegistry(/*IEnumerable<IConverterFamily> families*/)
    {
      var families = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetAllInstances<IConverterFamily>();
      _families.AddRange(families);

      _converters = new Cache<Type, ValueConverter>(t =>
      {
        var family = _families.Find(x => x.Matches(t));
        if (family == null) return null;

        return family.Build(this, t);
      });

      AddPolicies();
    }

    public ValueConverter this[Type type] { get { return _converters[type]; } }

    private void AddPolicies()
    {
      If(t => TypeDescriptor.GetConverter(t).CanConvertTo(t)).Use((r, type) => x => BasicConvert(type, x.Value));
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

      var converter = TypeDescriptor.GetConverter(type);

      return converter.ConvertFrom(original);
    }
  }
}
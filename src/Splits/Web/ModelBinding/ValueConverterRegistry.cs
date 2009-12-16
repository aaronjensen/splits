using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Splits.Web.ModelBinding
{
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
}
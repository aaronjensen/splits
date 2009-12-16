using System;

namespace Splits.Web.ModelBinding
{
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
}
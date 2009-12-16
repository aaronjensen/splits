using System;

namespace Splits.Web.ModelBinding.DefaultConverterFamilies
{
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
}
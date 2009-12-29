using System;

namespace Splits.Web.ModelBinding.DefaultConverterFamilies
{
  public class EnumFamily : IConverterFamily
  {
    public bool Matches(Type type)
    {
      return type.IsEnum;
    }

    public ValueConverter Build(IValueConverterRegistry registry, Type type)
    {
      return x => Enum.Parse(type, x.Value.ToString());
    }
  }
}
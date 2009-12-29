using System;

namespace Splits.Web.ModelBinding.DefaultConverterFamilies
{
  public class DateTimeFamily : IConverterFamily
  {
    public bool Matches(Type type)
    {
      return type == typeof(DateTime);
    }

    public ValueConverter Build(IValueConverterRegistry registry, Type type)
    {
      return x => ValueConverterRegistry.BasicConvert(typeof(DateTime), x.Value);
    }
  }
}
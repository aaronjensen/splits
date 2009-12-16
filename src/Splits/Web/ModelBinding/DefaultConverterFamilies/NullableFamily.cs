using System;

namespace Splits.Web.ModelBinding.DefaultConverterFamilies
{
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
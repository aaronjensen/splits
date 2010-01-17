using System;
using System.Globalization;

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
      return x => {
        long ticks;
        if (x.Value == null)
          return null;
        if (long.TryParse(x.Value.ToString(), out ticks))
          return new DateTime(ticks, DateTimeKind.Utc);
        return DateTime.Parse(x.Value.ToString(), CultureInfo.CurrentUICulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
      };
    }
  }
}
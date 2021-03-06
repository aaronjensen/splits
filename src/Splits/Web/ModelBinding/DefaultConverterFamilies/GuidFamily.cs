using System;

namespace Splits.Web.ModelBinding.DefaultConverterFamilies
{
  public class GuidFamily : IConverterFamily
  {
    public bool Matches(Type type)
    {
      return type == typeof(Guid);
    }

    public ValueConverter Build(IValueConverterRegistry registry, Type type)
    {
      return x => x.Value == null ? Guid.Empty : new Guid(x.Value.ToString());
    }
  }
}
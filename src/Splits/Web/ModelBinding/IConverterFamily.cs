using System;

namespace Splits.Web.ModelBinding
{
  public interface IConverterFamily
  {
    bool Matches(Type type);
    ValueConverter Build(IValueConverterRegistry registry, Type type);
  }
}
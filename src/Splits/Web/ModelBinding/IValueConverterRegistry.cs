using System;

namespace Splits.Web.ModelBinding
{
  public interface IValueConverterRegistry
  {
    ValueConverter this[Type type] { get; }
  }
}
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Splits.Web
{
  public interface ITypeDescriptorRegistry
  {
    IDictionary<string, PropertyInfo> GetPropertiesFor<TYPE>();
    IDictionary<string, PropertyInfo> GetPropertiesFor(Type itemType);
    void ForEachWritableProperty(Type itemType, Action<PropertyInfo> action);
  }
}
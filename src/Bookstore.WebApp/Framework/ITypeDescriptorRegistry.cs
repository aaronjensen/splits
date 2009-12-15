using System;
using System.Collections.Generic;
using System.Reflection;

namespace Bookstore.WebApp.Framework
{
  public interface ITypeDescriptorRegistry
  {
    IDictionary<string, PropertyInfo> GetPropertiesFor<TYPE>();
    IDictionary<string, PropertyInfo> GetPropertiesFor(Type itemType);
    void ForEachProperty(Type itemType, Action<PropertyInfo> action);
  }
}
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace Splits.Web
{
  public class TypeDescriptorRegistry : ITypeDescriptorRegistry
  {
    private readonly Cache<Type, IDictionary<string, PropertyInfo>> _cache;

    public TypeDescriptorRegistry()
    {
      _cache = new Cache<Type, IDictionary<string, PropertyInfo>>(type => {
        var dict = new Dictionary<string, PropertyInfo>();

        foreach (var propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
          dict.Add(propertyInfo.Name, propertyInfo);
        }

        return dict;
      });
    }

    public IDictionary<string, PropertyInfo> GetPropertiesFor<TYPE>()
    {
      return GetPropertiesFor(typeof(TYPE));
    }

    public IDictionary<string, PropertyInfo> GetPropertiesFor(Type itemType)
    {
      return _cache[itemType];
    }

    public void ForEachWritableProperty(Type itemType, Action<PropertyInfo> action)
    {
      _cache[itemType].Values.Where(pi => pi.CanWrite).Each(action);
    }

    public void ClearAll()
    {
      _cache.ClearAll();
    }
  }
}
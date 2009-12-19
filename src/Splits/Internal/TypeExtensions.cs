using System;
using System.Collections.Generic;
using System.Linq;

namespace Splits.Internal
{
  public static class TypeExtensions
  {
    public static Type GetGenericArgumentInImplementationOf(this Type type, Type genericType)
    {
      if (!genericType.IsGenericTypeDefinition)
        throw new ArgumentException("genericType");
      foreach (var iface in type.FindInterfaces((a, b) => true, null))
      {
        if (iface.GetGenericTypeDefinition() == genericType)
        {
          return iface.GetGenericArguments().First();
        }
      }
      return null;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using Splits.Web;
using Splits.Web.StepHandlers;

namespace Splits
{
  public static class SplitsServices
  {
    public static IEnumerable<KeyValuePair<Type, Type>> Singletons
    {
      get
      {
        yield return Self(typeof(FrameworkStartup));
        yield return Self(typeof(SplitsController));
        yield return Self(typeof(StepProvider));
        yield return Self(typeof(TypeDescriptorRegistry));
        yield return Self(typeof(StepInvoker));
        yield return Self(typeof(StepHandlerLocator));
      }
    }

    private static KeyValuePair<Type, Type> Self(Type type)
    {
      return new KeyValuePair<Type, Type>(type, type);
    }

    public static IEnumerable<KeyValuePair<Type, Type>> Transients
    {
      get
      {
        foreach (var pair in AllInAssemblyGeneric(typeof(SplitsServices).Assembly, typeof(IStepHandler<>)))
        {
          yield return pair;
        }
      }
    }

    private static IEnumerable<KeyValuePair<Type, Type>> AllInAssemblyGeneric(Assembly assembly, Type type)
    {
      return assembly.GetExportedTypes()
        .Where(service => service.IsClass)
        .SelectMany(
        service => service.GetInterfaces().Select(@interface => new KeyValuePair<Type, Type>(@interface, service)))
        .Where(pair => pair.Key.IsGenericType && pair.Key.GetGenericTypeDefinition() == type);
    }

    private static IEnumerable<KeyValuePair<Type, Type>> AllInAssembly(Assembly assembly, Type type, string @namespace)
    {
      return assembly.GetExportedTypes()
        .Where(service => service.IsClass)
        .SelectMany(
        service => service.GetInterfaces().Select(@interface => new KeyValuePair<Type, Type>(@interface, service)))
        .Where(pair => pair.Value.Namespace.StartsWith(@namespace));
    }
  }
}

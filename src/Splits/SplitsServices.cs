using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Splits.Application.Impl;
using Splits.Internal;
using Splits.Queries;
using Splits.Web;
using Splits.Web.ModelBinding;
using Splits.Web.ModelBinding.DefaultConverterFamilies;
using Splits.Web.Spark;
using Splits.Web.StepHandlers;
using Splits.Web.Validation;

namespace Splits
{
  public class BadServiceProvider : IServiceProvider
  {
    public object GetService(Type serviceType)
    {
      throw new NotImplementedException();
    }
  }
  
  public static class SplitsServices
  {
    public static IEnumerable<KeyValuePair<Type, Type>> Singletons
    {
      get
      {
        yield return Self(typeof(FrameworkStartup));
        yield return Self(typeof(EventOrdering));
        yield return Self(typeof(SplitsController));
        yield return Self(typeof(StepProvider));
        yield return Self(typeof(TypeDescriptorRegistry));
        yield return Self(typeof(StepInvoker));
        yield return Self(typeof(StepHandlerLocator));
        yield return Self(typeof(StandardModelBinder));
        yield return Self(typeof(ValueConverterRegistry));
        yield return Self(typeof(ModelValidator));
        yield return Self(typeof(CommandInvoker));
        yield return Self(typeof(CommandHandlerLocator));
        yield return Self(typeof(QueryInvoker));
        yield return Self(typeof(QueryHandlerLocator));
        yield return Self(typeof(SparkViewFactory));
        yield return Self(typeof(ViewRenderer));
        yield return Self(typeof(BadServiceProvider));
        yield return Self(typeof(IsAuthenticatedQueryHandler));
        yield return Self(typeof(QueryBinder));

        foreach (var pair in AllInAssembly(typeof(SplitsServices).Assembly, typeof(IConverterFamily), typeof(NullableFamily).Namespace))
        {
          yield return pair;
        }
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

    public static IEnumerable<KeyValuePair<Type, Type>> AllInAssemblyGeneric(Assembly assembly, Type type)
    {
      return assembly.GetExportedTypes()
        .Where(service => service.IsClass)
        .SelectMany(service => service.GetInterfaces().Select(@interface => new KeyValuePair<Type, Type>(@interface, service)))
        .Where(pair => pair.Key.IsGenericType && pair.Key.GetGenericTypeDefinition() == type);
    }

    public static IEnumerable<KeyValuePair<Type, Type>> AllInAssembly(Assembly assembly, Type type)
    {
      return assembly.GetExportedTypes()
        .Where(service => service.IsClass && type.IsAssignableFrom(service))
        .SelectMany(service => service.GetInterfaces().Select(@interface => new KeyValuePair<Type, Type>(@interface, service)));
    }

    public static IEnumerable<KeyValuePair<Type, Type>> AllInAssembly(Assembly assembly, Type type, string @namespace)
    {
      return assembly.GetExportedTypes()
        .Where(service => service.IsClass && type.IsAssignableFrom(service))
        .SelectMany(service => service.GetInterfaces().Select(@interface => new KeyValuePair<Type, Type>(@interface, service)))
        .Where(pair => pair.Value.Namespace.StartsWith(@namespace));
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bookstore.WebApp.Framework;
using Bookstore.WebApp.Framework.StepHandlers;
using Ninject.Modules;

namespace Bookstore.WebApp
{
  public class WebAppServices : NinjectModule
  {
    public override void Load()
    {
      BindTo<WebAppStartup>().InSingletonScope();
      BindTo<Cache>().InSingletonScope();
      BindTo<HttpCacheProvider>().InSingletonScope();

      BindTo<FrameworkStartup>().InSingletonScope();
      BindTo<WiftController>().InTransientScope();
      BindTo<StepProvider>().InSingletonScope();
      BindTo<TypeDescriptorRegistry>().InSingletonScope();
      BindTo<StepInvoker>().InSingletonScope();
      BindTo<StepHandlerLocator>().InSingletonScope();

      BindAllInAssemblyGeneric(GetType().Assembly, typeof(IStepHandler<>));
      BindAllInAssembly(GetType().Assembly, typeof(IRule), "Bookstore.WebApp.Rules");
    }

    public void BindAllInAssemblyGeneric(Assembly assembly, Type type)
    {
      foreach (var pair in assembly.GetExportedTypes()
        .Where(service => service.IsClass)
        .SelectMany(service => service.GetInterfaces().Select(@interface => new KeyValuePair<Type, Type>(service, @interface)))
        .Where(pair => pair.Value.IsGenericType && pair.Value.GetGenericTypeDefinition() == type))
      {
        Bind(pair.Value).To(pair.Key).InTransientScope();
      }
    }

    public void BindAllInAssembly(Assembly assembly, Type type, string @namespace)
    {
      foreach (var pair in assembly.GetExportedTypes()
        .Where(service => service.IsClass)
        .SelectMany(service => service.GetInterfaces().Select(@interface => new KeyValuePair<Type, Type>(service, @interface)))
        .Where(pair => pair.Key.Namespace.StartsWith(@namespace)))
      {
        Bind(pair.Value).To(pair.Key).InTransientScope();
      }
    }
  }

}

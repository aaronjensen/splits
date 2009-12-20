using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bookstore.WebApp.Rules;
using Splits;
using Splits.Web;
using Splits.Web.StepHandlers;
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

      foreach (var pair in SplitsServices.Singletons)
      {
        Bind(pair.Key).To(pair.Value).InSingletonScope();
      }

      foreach (var pair in SplitsServices.Transients)
      {
        Bind(pair.Key).To(pair.Value).InTransientScope();
      }

      BindAllInAssembly(GetType().Assembly, typeof(IRule), typeof(BookstoreSetupRules).Namespace);
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

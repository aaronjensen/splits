using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bookstore.Application.Impl.Framework;
using Bookstore.Application.Impl.Framework.Impl;
using Ninject.Modules;

namespace Bookstore.Application.Impl
{
  public class ApplicationServices : NinjectModule
  {
    public override void Load()
    {
      BindTo<ApplicationStartup>().InSingletonScope();

      BindTo<QueryInvoker>().InSingletonScope();
      BindTo<CommandInvoker>().InSingletonScope();
      BindTo<CommandHandlerLocator>().InSingletonScope();
      BindTo<QueryHandlerLocator>().InSingletonScope();

      BindAllInAssembly(GetType().Assembly, typeof(ICommandHandler<,>));
      BindAllInAssembly(GetType().Assembly, typeof(IQueryHandler<,>));
      BindAllInAssembly(GetType().Assembly, typeof(IDomainEventHandler<>));
    }

    public void BindAllInAssembly(Assembly assembly, Type type)
    {
      foreach (var pair in assembly.GetExportedTypes()
        .Where(service => service.IsClass)
        .SelectMany(service => service.GetInterfaces().Select(@interface => new KeyValuePair<Type, Type>(service, @interface)))
        .Where(pair => pair.Value.IsGenericType && pair.Value.GetGenericTypeDefinition() == type))
      {
        Bind(pair.Value).To(pair.Key).InTransientScope();
      }
    }
  }
}

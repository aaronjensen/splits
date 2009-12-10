using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bookstore.WebApp;
using CommonServiceLocator.NinjectAdapter;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Modules;

namespace Bookstore.WebSite
{
  public static class Container
  {
    static readonly StandardKernel _container = new StandardKernel();

    public static IKernel Start()
    {
      _container.Load(new INinjectModule[]
      {
        new WebAppServices()
      });

      var adapter = new NinjectServiceLocator(_container);
      ServiceLocator.SetLocatorProvider(() => adapter);

      return _container;
    }
  }
}

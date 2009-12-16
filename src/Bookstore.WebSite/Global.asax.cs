using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bookstore.Application.Impl;
using BookStore.WebApp;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Web.Mvc;
using Splits;

namespace Bookstore.WebSite
{
  public class MvcApplication : NinjectHttpApplication
  {
    protected override void OnApplicationStarted()
    {
      RegisterAllControllersIn(typeof(WebAppServices).Assembly);
      RegisterAllControllersIn(typeof(SplitsServices).Assembly);
      AreaRegistration.RegisterAllAreas();

      ServiceLocator.Current.GetInstance<ApplicationStartup>().Start();
      ServiceLocator.Current.GetInstance<WebAppStartup>().Start(RouteTable.Routes);
    }

    protected override IKernel CreateKernel()
    {
      var kernel = Container.Start();

      return kernel;
    }
  }
}
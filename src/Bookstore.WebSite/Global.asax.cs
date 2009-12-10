﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Bookstore.WebApp;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using Ninject.Web.Mvc;

namespace Bookstore.WebSite
{
  public class MvcApplication : NinjectHttpApplication
  {
    protected override void OnApplicationStarted()
    {
      RegisterAllControllersIn(typeof(WebAppServices).Assembly);
      RegisterRoutes(RouteTable.Routes);
      AreaRegistration.RegisterAllAreas();

      ServiceLocator.Current.GetInstance<WebAppStartup>().Start();
    }

    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
        "Default",                                              // Route name
        "{controller}/{action}/{id}",                           // URL with parameters
        new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
        );
    }

    protected override IKernel CreateKernel()
    {
      var kernel = Container.Start();

      return kernel;
    }
  }
}
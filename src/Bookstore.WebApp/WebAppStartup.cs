using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Bookstore.WebApp.Framework;
using Spark.Web.Mvc;

namespace Bookstore.WebApp
{
  public class WebAppStartup
  {
    readonly Routing _routing;
    readonly FrameworkStartup _frameworkStartup;

    public WebAppStartup(Routing routing, FrameworkStartup frameworkStartup)
    {
      _routing = routing;
      _frameworkStartup = frameworkStartup;
    }

    public void Start(RouteCollection routes)
    {
      ViewEngines.Engines.Add(new SparkViewFactory());
      _routing.RegisterRoutes(routes);
      _frameworkStartup.Start();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.Web.Mvc;

namespace Bookstore.WebApp
{
  public class WebAppStartup
  {
    readonly Routing _routing;

    public WebAppStartup(Routing routing)
    {
      _routing = routing;
    }

    public void Start(RouteCollection routes)
    {
      ViewEngines.Engines.Add(new SparkViewFactory());
      _routing.RegisterRoutes(routes);
    }
  }
}

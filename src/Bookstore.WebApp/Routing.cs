using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Bookstore.WebApp;
using Machine.UrlStrong.Mvc;
using Splits.Web.Routing;

namespace Bookstore.WebApp
{
  public class Routing
  {
    public void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute<Urls.Root>();
      routes.MapRoute<Urls.Root.Bookstore_setup>();
      routes.MapRoute<Urls.Root.Test>();
    }
  }
}
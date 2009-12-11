using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Bookstore.WebApp.Controllers;
using Machine.UrlStrong.Mvc;

namespace Bookstore.WebApp
{
  public class Routing
  {
    public void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRouteTo<HomeController>(Urls.root, x => x.Index());
      routes.MapRouteTo<BookStoreSetupController>(Urls.root.bookstore_setup, x => x.BookStoreSetup());
    }
  }
}

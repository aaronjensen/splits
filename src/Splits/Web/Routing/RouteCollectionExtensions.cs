using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Machine.UrlStrong;

namespace Splits.Web.Routing
{
  public static class RouteCollectionExtensions
  {
    public static Route MapRoute<TUrl>(this RouteCollection routes) where TUrl : IUrl
    {
      return routes.MapRoute(typeof(TUrl));
    }

    public static Route MapRoute(this RouteCollection routes, Type urlType)
    {
      if (routes == null) throw new ArgumentNullException("routes");

      var attribute = (StrongUrlAttribute)urlType.GetCustomAttributes(false).Where(x => x is StrongUrlAttribute).FirstOrDefault();

      if (attribute == null)
      {
        throw new ArgumentException(urlType + " is not a Strong Url, at least it doesn't have the StrongUrl attribute. Make sure you're using the latest UrlStrong and that this is the endpoint of a url i.e. you defined it as supporting GET or POST in your .urls config");
      }

      var url = attribute.ParameterizedUrl;
      while (url.Length > 0 && (url[0] == '/' || url[0] == '~'))
      {
        url = url.Substring(1);
      }
      
      var route = new Route(url, new MvcRouteHandler());
      route.Defaults = new RouteValueDictionary();
      route.Constraints = new RouteValueDictionary();
      route.DataTokens = new RouteValueDictionary();
      route.Defaults.Add("action", urlType.ToString());
      route.DataTokens.Add("urlType", urlType);
      route.Defaults.Add("controller", "Splits");

      routes.Add(urlType.ToString(), route);

      return route;
    }

    public static void MapAllStrongUrls(this RouteCollection routes, Type type)
    {
      foreach (var nested in type.GetNestedTypes())
      {
        if (nested.GetCustomAttributes(typeof(StrongUrlAttribute), false).Length > 0)
        {
          routes.MapRoute(nested);
        }
        routes.MapAllStrongUrls(nested);
      }
    }

    public static void MapAllStrongUrls<T>(this RouteCollection routes)
    {
      routes.MapAllStrongUrls(typeof(T));
    }
  }
}

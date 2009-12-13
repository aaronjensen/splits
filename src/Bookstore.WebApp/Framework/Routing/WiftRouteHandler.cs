using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Web.SessionState;
using Machine.UrlStrong;

namespace Bookstore.WebApp.Framework.Routing
{
  public class WiftHandler : IHttpAsyncHandler, IRequiresSessionState
  {
    readonly RequestContext _requestContext;

    public WiftHandler(RequestContext requestContext)
    {
      _requestContext = requestContext;
    }

    public void ProcessRequest(HttpContext context)
    {
      ProcessRequest(new HttpContextWrapper(context));
    }

    protected void ProcessRequest(HttpContextBase context)
    {
      _requestContext.RouteData.GetRequiredString("urlType");
    }

    public bool IsReusable
    {
      get { return false; }
    }

    public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
    {
      throw new NotImplementedException();
    }

    public void EndProcessRequest(IAsyncResult result)
    {
      throw new NotImplementedException();
    }
  }

  public class WiftRouteHandler : IRouteHandler
  {
    public IHttpHandler GetHttpHandler(RequestContext requestContext)
    {
      return new WiftHandler(requestContext);
    }
  }

  public static class RouteCollectionExtensions
  {
    public static Route MapRoute<TUrl>(this RouteCollection routes) where TUrl : IUrl
    {
      return routes.MapRoute(typeof(TUrl));
    }

    public static Route MapRoute(this RouteCollection routes, Type urlType)
    {
      if (routes == null) throw new ArgumentNullException("routes");
      
      var route = new Route("", new WiftRouteHandler());
      route.Defaults = new RouteValueDictionary();
      route.Constraints = new RouteValueDictionary();
      route.DataTokens = new RouteValueDictionary();
      route.Defaults.Add("urlType", urlType.ToString());

      routes.Add(urlType.ToString(), route);

      return route;
    }
  }
}

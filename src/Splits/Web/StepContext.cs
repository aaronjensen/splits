using System;
using System.Web;
using System.Web.Routing;
using System.Linq;

namespace Splits.Web
{
  public class StepContext
  {
    readonly RequestContext _requestContext;
    readonly string _urlStrongPath;

    public RequestContext RequestContext
    {
      get { return _requestContext; }
    }

    public string UrlStrongPath
    {
      get { return _urlStrongPath; }
    }

    public StepContext(RequestContext requestContext, Type urlType)
    {
      _requestContext = requestContext;
      _urlStrongPath = UrlStrongPathFromUrlsType(urlType);
    }

    public HttpResponseBase Response
    {
      get { return RequestContext.HttpContext.Response; }
    }

    public HttpRequestBase Request
    {
      get { return RequestContext.HttpContext.Request; }
    }

    static string UrlStrongPathFromUrlsType(Type urlType)
    {
      var parts = urlType.FullName.Split('+').Skip(1).Select(s => s.ToLower()).ToArray();
      return String.Join("/", parts);
    }
  }
}
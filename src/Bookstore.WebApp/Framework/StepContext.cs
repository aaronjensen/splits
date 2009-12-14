using System;
using System.Web;
using System.Web.Routing;
using Bookstore.Application.Framework;

namespace Bookstore.WebApp.Framework
{
  public class StepContext
  {
    readonly RequestContext _requestContext;

    public RequestContext RequestContext
    {
      get { return _requestContext; }
    }

    public StepContext(RequestContext requestContext)
    {
      _requestContext = requestContext;
    }

    public HttpResponseBase Response
    {
      get { return RequestContext.HttpContext.Response; }
    }

    public HttpRequestBase Request
    {
      get { return RequestContext.HttpContext.Request; }
    }
  }
}
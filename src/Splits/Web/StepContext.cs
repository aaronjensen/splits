using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using Splits.Application;

namespace Splits.Web
{
  public class StepContext
  {
    readonly RequestContext _requestContext;
    readonly string _urlStrongPath;
    readonly Dictionary<Identifier, IQuery> _queryMap = new Dictionary<Identifier, IQuery>();
    readonly Dictionary<Guid, object> _queryResultMap = new Dictionary<Guid, object>();
    readonly Dictionary<Identifier, ICommand> _commandMap = new Dictionary<Identifier, ICommand>();
    readonly Dictionary<Guid, object> _commandResultMap = new Dictionary<Guid, object>();
    Identifier _lastQuery;

    public IDictionary<Identifier, IQuery> QueryMap
    {
      get { return _queryMap; }
    }

    public IDictionary<Guid, object> QueryResultMap
    {
      get { return _queryResultMap; }
    }

    public RequestContext RequestContext
    {
      get { return _requestContext; }
    }

    public string UrlStrongPath
    {
      get { return _urlStrongPath; }
    }

    public object LastQueryResult
    {
      get { return _queryResultMap[_queryMap[_lastQuery].QueryId]; }
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

    public void AddQuery(IQuery query, object result, string name)
    {
      var identifier = new Identifier(name, query.GetType());

      if (_queryMap.ContainsKey(identifier))
        throw new ArgumentException(String.Format("Query of type {0} named \"{1}\" has already been registered.",
          query.GetType(), name));

      _queryMap[identifier] = query;
      _queryResultMap[query.QueryId] = result;
      _lastQuery = identifier;
    }

    public void AddQuery<TResult>(IQuery<TResult> query, TResult result, string name)
    {
      AddQuery((IQuery)query, (object)result, name);
    }

    public TQuery GetQuery<TQuery>() where TQuery : class
    {
      return GetQuery<TQuery>("");
    }

    public TQuery GetQuery<TQuery>(string name) where TQuery : class
    {
      return _queryMap[new Identifier(name, typeof(TQuery))] as TQuery;
    }

    public TResult GetResult<TResult>(IQuery<TResult> query) where TResult : class
    {
      return _queryResultMap[query.QueryId] as TResult;
    }

    public void Fill(ViewDataDictionary viewData)
    {
      foreach (var query in _queryMap)
      {
        var result = _queryResultMap[query.Value.QueryId];
        viewData[query.Key.Name] = result;
      }
    }
  }
}
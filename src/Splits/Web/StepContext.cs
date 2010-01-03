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
    readonly Dictionary<Identifier, IQuery> _queryMap = new Dictionary<Identifier, IQuery>();
    readonly Dictionary<Guid, object> _queryResultMap = new Dictionary<Guid, object>();
    readonly Dictionary<Identifier, ICommand> _commandMap = new Dictionary<Identifier, ICommand>();
    readonly Dictionary<Guid, object> _commandResultMap = new Dictionary<Guid, object>();
    readonly string _urlStrongPath;
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

    public HttpResponseBase Response
    {
      get { return RequestContext.HttpContext.Response; }
    }

    public HttpRequestBase Request
    {
      get { return RequestContext.HttpContext.Request; }
    }

    public StepContext(RequestContext requestContext, Type urlType)
    {
      _requestContext = requestContext;
      _urlStrongPath = requestContext.UrlStrongPathFromRoute();
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

    public void Fill(ViewDataDictionary viewData)
    {
      foreach (var query in _queryMap)
      {
        var result = _queryResultMap[query.Value.QueryId];
        viewData[query.Key.Name] = result;
      }
    }

    public T Get<T>() where T: class
    {
      var queriesAndCommands = _queryResultMap.Values.Union(_commandResultMap.Values);
      return (T)queriesAndCommands.Single(r => typeof(T).IsInstanceOfType(r));
    }
  }
}
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
    object _lastQueryOrCommandResult;

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

    public object LastQueryOrCommandResult
    {
      get { return _lastQueryOrCommandResult; }
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

    public void AddCommand(ICommand command, object result, string name)
    {
      var identifier = new Identifier(name, command.GetType());

      if (_commandMap.ContainsKey(identifier))
        throw new ArgumentException(String.Format("Command of type {0} named \"{1}\" has already been registered.",
          command.GetType(), name));

      _commandMap[identifier] = command;
      _commandResultMap[Guid.NewGuid()] = result;
      _lastQueryOrCommandResult = result;
    }

    public void AddQuery(IQuery query, object result, string name)
    {
      var identifier = new Identifier(name, query.GetType());

      if (_queryMap.ContainsKey(identifier))
        throw new ArgumentException(String.Format("Query of type {0} named \"{1}\" has already been registered.",
          query.GetType(), name));

      _queryMap[identifier] = query;
      _queryResultMap[query.QueryId] = result;
      _lastQueryOrCommandResult = result;
    }

    public void Fill(ViewDataDictionary viewData)
    {
      foreach (var query in _queryMap)
      {
        var result = _queryResultMap[query.Value.QueryId];
        viewData[query.Key.Name] = result;
      }
    }

    public T TryGet<T>() where T : class
    {
      var queriesAndCommands = _commandMap.Select(r => r.Value).Cast<object>().Union(_queryResultMap.Values.Union(_commandResultMap.Values));
      var matching = queriesAndCommands.Where(r => typeof(T).IsInstanceOfType(r));
      if (matching.Any())
        return (T)matching.Single();
      return default(T);
    }

    public T Get<T>() where T : class
    {
      var gotten = TryGet<T>();
      if (gotten == default(T))
        throw new InvalidOperationException("No value of type: " + typeof(T));
      return gotten;
    }
  }
}
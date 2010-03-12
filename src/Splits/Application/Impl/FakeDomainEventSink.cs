using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splits.Application.Impl
{
  public class FakeDomainEventSink : IDomainEventSink
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof (FakeDomainEventSink));
    readonly static IList<Delegate> _actions = new List<Delegate>();
    readonly static IList<RaisedEvent> _raisedEvents = new List<RaisedEvent>();

    public static void Register<T>(Action<T> callback) where T : IDomainEvent
    {
      _actions.Add(callback);
    }

    public void Begin()
    {
    }

    public void Raise<T>(T args) where T : IDomainEvent
    {
      _raisedEvents.Add(new RaisedEvent(typeof(T), args));
      _log.Info(args);

      if (_actions != null)
      {
        foreach (var action in _actions)
        {
          if (action is Action<T>)
          {
            ((Action<T>)action)(args);
          }
        }
      }
    }

    public void Commit()
    {
    }

    public static void ShouldNotHaveRaised<T>(params Func<T, bool>[] criteria) where T : IDomainEvent
    {
      if (_raisedEvents.Where(x => typeof(T) == x.Type).Where(x => criteria.Aggregate(true, (i, c) => i && c((T)x.Args))).Any())
      {
        throw new Exception("Should not have raised event matching critera");
      }
    }

    public static void ShouldHaveRaised<T>(params Func<T, bool>[] criteria) where T : IDomainEvent
    {
      if (!_raisedEvents.Where(x => typeof(T) == x.Type).Where(x => criteria.Aggregate(true, (i, c) => i && c((T)x.Args))).Any())
      {
        throw new Exception("Should have raised event matching critera");
      }
    }

    public static void ShouldNotHaveRaised<T>(T args) where T : IDomainEvent
    {
      var raisedEvent = new RaisedEvent(typeof(T), args);
      if (_raisedEvents.Contains(raisedEvent))
      {
        throw new Exception("Should not have raised " + raisedEvent);
      }
    }

    public static void ShouldHaveRaised<T>(params Action<T>[] criteria) where T : IDomainEvent
    {
      var errors = new List<string>();
      foreach (var de in _raisedEvents.Where(x => typeof(T) == x.Type))
      {
        foreach (var action in criteria)
        {
          try
          {
            action((T)de.Args);
          }
          catch (Exception error)
          {
            if (de.Args != null)
              errors.Add(de.Args + " " + error.Message);
            else
              errors.Add("(null) " + error.Message);
          }
        }
      }
      if (errors.Distinct().Any())
      {
        var message = new StringBuilder();
        message.AppendLine("Expected domain event should have been raised:");
        foreach (var m in errors)
        {
          message.AppendLine(m);
        }
        throw new Exception(message.ToString());
      }
    }

    public static void ShouldHaveRaised<T>(T args) where T : IDomainEvent
    {
      var raisedEvent = new RaisedEvent(typeof(T), args);
      if (!_raisedEvents.Contains(raisedEvent))
      {
        throw new Exception("Should have raised " + raisedEvent);
      }
    }

    public static void Start()
    {
      DomainEvent.SetDomainEventSinkProvider(() => new FakeDomainEventSink());
    }

    public static void Reset()
    {
      _actions.Clear();
      _raisedEvents.Clear();
    }
  }

  public class RaisedEvent
  {
    readonly Type _type;
    readonly IDomainEvent _args;

    public Type Type
    {
      get { return _type; }
    }

    public IDomainEvent Args
    {
      get { return _args; }
    }

    public RaisedEvent(Type type, IDomainEvent args)
    {
      _type = type;
      _args = args;
    }

    public bool Equals(RaisedEvent other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(other._type, _type) && Equals(other._args, _args);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != typeof(RaisedEvent)) return false;
      return Equals((RaisedEvent)obj);
    }

    public override Int32 GetHashCode()
    {
      return ((_type != null ? _type.GetHashCode() : 0) * 397) ^ (_args != null ? _args.GetHashCode() : 0);
    }

    public override string ToString()
    {
      return string.Format("Type: {0}, Args: {1}", _type, _args);
    }
  }
}
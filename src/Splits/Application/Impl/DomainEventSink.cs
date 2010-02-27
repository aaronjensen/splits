using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Splits.Internal;

namespace Splits.Application.Impl
{
  public class DomainEventSink : IDomainEventSink
  {
    readonly static log4net.ILog _log = log4net.LogManager.GetLogger(typeof (DomainEventSink));
    readonly EventOrdering _ordering;

    public DomainEventSink(EventOrdering ordering)
    {
      _ordering = ordering;
    }

    public void Begin()
    {
    }

    public void Raise<TEvent>(TEvent e) where TEvent : IDomainEvent
    {
      Raise(typeof(TEvent), e);
    }

    public void Raise(Type eventType, IDomainEvent e)
    {
      var locator = ServiceLocator.Current;
      if (locator == null) return;
      var order = _ordering.Ordering().ToList();
      foreach (var type in DomainEventTypes(eventType))
      {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(type);
        var wrapperType = typeof(Invoker<>).MakeGenericType(type);
        var handlers = locator.GetAllInstances(handlerType).Select(h => new {
          Type = h.GetType(),
          Order = order.IndexOf(h.GetType()),
          Handler =(IDomainEventHandler<IDomainEvent>)Activator.CreateInstance(wrapperType, h)
        }).
        Select(h => new {
          Type = h.Type,
          Order = h.Order == -1 ? order.Count : h.Order,
          h.Handler
        }).
        OrderBy(h => h.Order).
        ToList();
        foreach (var handler in handlers)
        {
          _log.Info("Raising " + e + " - " + handler.Type);
          handler.Handler.Handle(e);
        }
      }
    }

    public void Commit()
    {
    }

    static IEnumerable<Type> DomainEventTypes(Type type)
    {
      if (!typeof(IDomainEvent).IsAssignableFrom(type))
        yield break;
      yield return type;
      foreach (var basetype in DomainEventTypes(type.BaseType))
        yield return basetype;
    }

    class Invoker<T> : IDomainEventHandler<IDomainEvent> where T : IDomainEvent
    {
      readonly IDomainEventHandler<T> _target;

      public Invoker(IDomainEventHandler<T> target)
      {
        _target = target;
      }

      public void Handle(IDomainEvent e)
      {
        _target.Handle((T)e);
      }
    }
  }
}
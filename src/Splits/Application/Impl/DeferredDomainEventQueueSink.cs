using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Practices.ServiceLocation;

namespace Splits.Application.Impl
{
  public class DeferredDomainEventQueueSink : IDomainEventSink
  {
    public void Begin()
    {
      EventQueue().Clear();
    }

    public void Raise<TEvent>(TEvent args) where TEvent : IDomainEvent
    {
      EventQueue().Enqueue(new RaisedEvent(typeof(TEvent), args));
    }

    public void Commit()
    {
      var locator = ServiceLocator.Current;
      if (locator == null) return;
      var eventQueue = EventQueue();
      while (eventQueue.Count > 0)
      {
        var raised = eventQueue.Dequeue();
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(raised.Type);
        var wrapperType = typeof(Invoker<>).MakeGenericType(raised.Type);
        foreach (var handler in locator.GetAllInstances(handlerType))
        {
          var wrapper = (IDomainEventHandler<IDomainEvent>)Activator.CreateInstance(wrapperType, handler);
          wrapper.Handle(raised.Args);
        }
      }
    }

    [ThreadStatic]
    static Queue<RaisedEvent> _eventQueue;

    static Queue<RaisedEvent> EventQueue()
    {
      if (HttpContext.Current == null)
      {
        _eventQueue = new Queue<RaisedEvent>();
        return _eventQueue;
      }
      var items = HttpContext.Current.Items;
      var key = typeof(DeferredDomainEventQueueSink).Name;
      if (items[key] == null)
      {
        items[key] = new Queue<RaisedEvent>();
      }
      return (Queue<RaisedEvent>)items[key];
    }

    class Invoker<T> : IDomainEventHandler<IDomainEvent> where T : IDomainEvent
    {
      readonly IDomainEventHandler<T> _target;

      public Invoker(IDomainEventHandler<T> target)
      {
        _target = target;
      }

      public void Handle(IDomainEvent @event)
      {
        _target.Handle((T)@event);
      }
    }
  }
}
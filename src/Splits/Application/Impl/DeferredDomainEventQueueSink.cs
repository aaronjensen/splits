using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.Practices.ServiceLocation;

namespace Splits.Application.Impl
{
  public class DeferredDomainEventQueueSink : IDomainEventSink
  {
    readonly DomainEventSink _sink = new DomainEventSink();

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
        _sink.Raise(raised.Type, raised.Args);
      }
    }

    [ThreadStatic]
    static Queue<RaisedEvent> _eventQueue;

    static Queue<RaisedEvent> EventQueue()
    {
      if (HttpContext.Current == null)
      {
        if (_eventQueue == null)
        {
          _eventQueue = new Queue<RaisedEvent>();
        }
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
  }
}
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using Splits.Internal;

namespace Splits.Application.Impl
{
  public class DeferredDomainEventQueueSink : IDomainEventSink
  {
    readonly DomainEventSink _sink;

    public DeferredDomainEventQueueSink(EventOrdering ordering)
    {
      _sink = new DomainEventSink(ordering);
    }

    public void Begin()
    {
      EventQueue().Clear();
    }

    public void Raise<TEvent>(TEvent e) where TEvent : IDomainEvent
    {
      EventQueue().Enqueue(new RaisedEvent(typeof(TEvent), e));
    }

    public virtual IEnumerable<IDomainEvent> Commit()
    {
      var locator = ServiceLocator.Current;
      if (locator == null)
      {
        return new IDomainEvent[0];
      }
      var eventQueue = EventQueue();
      var raisedEvents = new List<RaisedEvent>();
      while (eventQueue.Count > 0)
      {
        var raised = eventQueue.Dequeue();
        _sink.Raise(raised.Type, raised.Args);
        raisedEvents.Add(raised);
      }
      Commit(raisedEvents);
      return raisedEvents.Select(e => e.Args);
    }

    public virtual void Commit(IEnumerable<RaisedEvent> raisedEvents)
    {
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
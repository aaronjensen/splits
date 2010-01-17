using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;

namespace Splits.Application.Impl
{
  public class DomainEventSink : IDomainEventSink
  {
    public void Begin()
    {
    }

    public void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
      Raise(typeof(TEvent), @event);
    }

    public void Raise(Type eventType, IDomainEvent @event)
    {
      var locator = ServiceLocator.Current;
      if (locator == null) return;
      foreach (var type in DomainEventTypes(eventType))
      {
        var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(type);
        var wrapperType = typeof(Invoker<>).MakeGenericType(type);
        foreach (var handler in locator.GetAllInstances(handlerType))
        {
          var wrapper = (IDomainEventHandler<IDomainEvent>)Activator.CreateInstance(wrapperType, handler);
          wrapper.Handle(@event);
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

      public void Handle(IDomainEvent @event)
      {
        _target.Handle((T)@event);
      }
    }
  }
}
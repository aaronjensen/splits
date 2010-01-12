using System;
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
      var locator = ServiceLocator.Current;
      if (locator == null) return;
      foreach (var handler in locator.GetAllInstances<IDomainEventHandler<TEvent>>())
      {
        handler.Handle(@event);
      }
    }

    public void Commit()
    {
    }
  }
}
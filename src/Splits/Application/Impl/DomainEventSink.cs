using Microsoft.Practices.ServiceLocation;
using Splits;

namespace Splits.Application.Impl
{
  public class DomainEventSink : IDomainEventSink
  {
    public void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
      var locator = ServiceLocator.Current;
      if (locator != null)
      {
        foreach (var handler in locator.GetAllInstances<IDomainEventHandler<TEvent>>())
        {
          handler.Handle(@event);
        }
      }
    }
  }
}
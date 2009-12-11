using Microsoft.Practices.ServiceLocation;

namespace Bookstore.Application.Impl.Framework.Impl
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
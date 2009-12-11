namespace Bookstore.Application.Impl.Framework
{
  public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
  {
    void Handle(TEvent @event);
  }
}
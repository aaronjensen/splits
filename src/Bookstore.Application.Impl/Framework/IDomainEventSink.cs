namespace Bookstore.Application.Impl.Framework
{
  public interface IDomainEventSink
  {
    void Raise<TEvent>(TEvent args) where TEvent : IDomainEvent;
  }
}
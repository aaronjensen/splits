namespace Splits.Application
{
  public interface IDomainEventHandler<TEvent> where TEvent : IDomainEvent
  {
    void Handle(TEvent @event);
  }
}
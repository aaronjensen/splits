using Splits;

namespace Splits.Application
{
  public interface IDomainEventSink
  {
    void Raise<TEvent>(TEvent args) where TEvent : IDomainEvent;
  }
}
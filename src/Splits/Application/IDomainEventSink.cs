using Splits;

namespace Splits.Application
{
  public interface IDomainEventSink
  {
    void Begin();
    void Raise<TEvent>(TEvent args) where TEvent : IDomainEvent;
    void Commit();
  }
}
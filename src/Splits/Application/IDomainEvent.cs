using System;
using System.Collections.Generic;

namespace Splits.Application
{
  public interface IDomainEvent
  {
  }

  public static class DomainEvent
  {
    static Func<IDomainEventSink> _provider = () => { throw new InvalidOperationException(); };

    public static void Begin()
    {
      _provider().Begin();
    }

    public static void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
      _provider().Raise(@event);
    }

    public static IEnumerable<IDomainEvent> Commit()
    {
      return _provider().Commit();
    }

    public static void SetDomainEventSinkProvider(Func<IDomainEventSink> provider)
    {
      _provider = provider;
    }
  }
}
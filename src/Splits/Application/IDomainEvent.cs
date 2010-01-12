using System;
using System.Collections.Generic;
using Splits.Application.Impl;

namespace Splits.Application
{
  public interface IDomainEvent
  {
  }

  public static class DomainEvent
  {
    static readonly IDomainEventSink _sink = new DomainEventSink();
    static Func<IDomainEventSink> _provider = () => _sink;

    public static void Begin()
    {
      _provider().Begin();
    }

    public static void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
      _provider().Raise(@event);
    }

    public static void Commit()
    {
      _provider().Commit();
    }

    public static void SetDomainEventSinkProvider(Func<IDomainEventSink> provider)
    {
      _provider = provider;
    }
  }
}
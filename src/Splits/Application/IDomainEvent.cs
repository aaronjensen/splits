using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Splits;
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

    public static void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent
    {
      _provider().Raise(@event);
    }

    public static void SetDomainEventSinkProvider(Func<IDomainEventSink> provider)
    {
      _provider = provider;
    }
  }
}
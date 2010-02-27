using System;
using System.Collections.Generic;
using Splits.Internal;

namespace Splits.Application
{
  public interface IConfigureSplits
  {
    void Configure(Configurer configure);
  }

  public class Configurer
  {
    readonly EventOrdering _ordering;

    public Configurer(EventOrdering ordering)
    {
      _ordering = ordering;
    }

    public Configurer First<T>()
    {
      _ordering.Append(typeof(T));
      return this;
    }

    public Configurer Then<T>()
    {
      _ordering.Append(typeof(T));
      return this;
    }
  }
}

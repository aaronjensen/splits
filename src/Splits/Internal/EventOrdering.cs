using System;
using System.Collections.Generic;

namespace Splits.Internal
{
  public class EventOrdering
  {
    readonly List<Type> _types = new List<Type>();

    public void Append(Type type)
    {
      _types.Add(type);
    }

    public IEnumerable<Type> Ordering()
    {
      return _types;
    }
  }
}

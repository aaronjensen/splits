using System;
using System.Collections.Generic;

namespace Splits.Web
{
  public interface IModelBinder
  {
    bool Matches(Type type);
    BindResult Bind(Type type, IDictionary<string, object> data);
  }
}
using System;
using System.Collections.Generic;

namespace Splits.Web.ModelBinding
{
  public interface IModelBinder
  {
    bool Matches(Type type);
    BindResult Bind(Type type, IDictionary<string, object> data);
    BindResult Bind(Type type, IDictionary<string, object> data, string prefix);
  }
}
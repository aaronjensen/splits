using System;
using System.Collections.Generic;

namespace Bookstore.WebApp.Framework
{
  public interface IModelBinder
  {
    bool Matches(Type type);
    BindResult Bind(Type type, IDictionary<string, object> data);
  }
}
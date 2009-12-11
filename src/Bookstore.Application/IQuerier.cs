using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Framework;

namespace Bookstore.Application
{
  public interface IQuerier
  {
    TResult Get<TResult>(IQuery<TResult> query);
  }
}

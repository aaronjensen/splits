using System;
using System.Web;
using Bookstore.Application.Framework;

namespace Bookstore.WebApp.Framework
{
  public class StepContext
  {
    public HttpResponseBase Response
    {
      get;
      set;
    }

    public T Query<T>(IQuery<T> query)
    {
      throw new NotImplementedException();
    }

    public T Invoke<T>(ICommand<T> command) where T : ICommandResult
    {
      throw new NotImplementedException();
    }
  }
}
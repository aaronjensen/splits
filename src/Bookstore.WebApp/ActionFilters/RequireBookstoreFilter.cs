using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bookstore.Application.Queries;
using Bookstore.WebApp;
using Microsoft.Practices.ServiceLocation;
using Splits.Application;

namespace Bookstore.WebApp.ActionFilters
{
  public class RequireBookstoreFilter : ActionFilterAttribute
  {
    readonly ICache _cache;
    readonly IQueryInvoker _queryInvoker;

    public RequireBookstoreFilter()
    {
      _cache = ServiceLocator.Current.GetInstance<ICache>();
      _queryInvoker = ServiceLocator.Current.GetInstance<IQueryInvoker>();
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      var bookStore = _cache.BookStore;
      if (bookStore != null) return;

      bookStore = _queryInvoker.Get(new GetThisBookStore());

      if (bookStore != null)
      {
        _cache.BookStore = bookStore;
        return;
      }

      filterContext.Result = new RedirectResult(Urls.root.bookstore_setup.ToString());
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bookstore.Application;
using Bookstore.Application.Queries;
using Microsoft.Practices.ServiceLocation;

namespace Bookstore.WebApp.ActionFilters
{
  public class RequireBookstoreFilter : ActionFilterAttribute
  {
    readonly ICache _cache;
    readonly IQuerier _querier;

    public RequireBookstoreFilter()
    {
      _cache = ServiceLocator.Current.GetInstance<ICache>();
      _querier = ServiceLocator.Current.GetInstance<IQuerier>();
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      var bookStore = _cache.BookStore;
      if (bookStore != null) return;

      bookStore = _querier.Get(new GetThisBookStore());

      if (bookStore != null)
      {
        _cache.BookStore = bookStore;
        return;
      }

      filterContext.Result = new RedirectResult(Urls.root.bookstore_setup.ToString());
    }
  }
}

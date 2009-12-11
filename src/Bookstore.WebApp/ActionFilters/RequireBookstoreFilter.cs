using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;

namespace Bookstore.WebApp.ActionFilters
{
  public class RequireBookstoreFilter : ActionFilterAttribute
  {
    readonly ICache _cache;

    public RequireBookstoreFilter()
    {
      _cache = ServiceLocator.Current.GetInstance<ICache>();
    }

    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
      var bookStore = _cache.BookStore;
      if (bookStore == null)
      {
        
      }
    }
  }
}

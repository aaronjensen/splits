using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Bookstore.WebApp.ActionFilters;

namespace Bookstore.WebApp.Controllers
{
  [RequireBookstoreFilter]
  public class BookstoreController : Controller
  {
  }
}

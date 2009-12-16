using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BookStore.WebApp.ActionFilters;

namespace BookStore.WebApp.Controllers
{
  [RequireBookstoreFilter]
  public class BookstoreController : Controller
  {
  }
}

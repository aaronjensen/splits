using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Bookstore.WebApp.Controllers
{
  public class HomeController : BookstoreController
  {
    public ActionResult Index()
    {
      return View();
    }
  }
}

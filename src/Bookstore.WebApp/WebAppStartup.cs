using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.Web.Mvc;

namespace Bookstore.WebApp
{
  public class WebAppStartup
  {
    public void Start()
    {
      ViewEngines.Engines.Add(new SparkViewFactory());
    }
  }
}

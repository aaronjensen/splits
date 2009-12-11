using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Ninject.Modules;

namespace Bookstore.WebApp
{
  public class WebAppServices : NinjectModule
  {
    public override void Load()
    {
      BindTo<WebAppStartup>().InSingletonScope();
      BindTo<Cache>().InSingletonScope();
      BindTo<HttpCacheProvider>().InSingletonScope();
    }
  }

}

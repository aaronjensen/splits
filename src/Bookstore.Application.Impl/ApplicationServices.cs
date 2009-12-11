using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Impl.Framework.Impl;
using Ninject.Modules;

namespace Bookstore.Application.Impl
{
  public class ApplicationServices : NinjectModule
  {
    public override void Load()
    {
      BindTo<ApplicationStartup>();

      BindTo<Querier>();
      BindTo<Commander>();
      BindTo<CommandHandlerLocator>();
      BindTo<QueryHandlerLocator>();
    }
  }
}

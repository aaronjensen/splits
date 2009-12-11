using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Impl.Framework;
using Bookstore.Application.Model;
using Bookstore.Application.Queries;

namespace Bookstore.Application.Impl.QueryHandlers
{
  public class ThisBookStoreQueryHandler : IQueryHandler<ThisBookStoreQuery, BookStore>
  {
    public BookStore Handle(ThisBookStoreQuery query)
    {
      return null;
    }
  }
}

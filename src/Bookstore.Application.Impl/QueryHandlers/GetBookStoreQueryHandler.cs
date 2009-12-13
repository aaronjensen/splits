using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Impl.Framework;
using Bookstore.Application.Model;
using Bookstore.Application.Queries;

namespace Bookstore.Application.Impl.QueryHandlers
{
  public class IsBookStoreSetupQueryHandler : IQueryHandler<IsBookStoreSetup, bool>
  {
    public bool Handle(IsBookStoreSetup query)
    {
      return GetBookStoreQueryHandler.BookStore != null;
    }
  }

  public class GetBookStoreQueryHandler : IQueryHandler<GetThisBookStore, BookStore>
  {
    public static BookStore BookStore { get; set; }
    public BookStore Handle(GetThisBookStore query)
    {
      return BookStore;
    }
  }
}

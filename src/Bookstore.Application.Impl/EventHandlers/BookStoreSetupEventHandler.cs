using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Impl.Domain.Events;
using Bookstore.Application.Impl.Framework;
using Bookstore.Application.Impl.QueryHandlers;
using Bookstore.Application.Model;

namespace Bookstore.Application.Impl.EventHandlers
{
  public class BookStoreSetupEventHandler : IDomainEventHandler<BookStoreSetupEvent>
  {
    public void Handle(BookStoreSetupEvent @event)
    {
      ThisBookStoreQueryHandler.BookStore = new BookStore();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Impl.Framework;

namespace Bookstore.Application.Impl.Domain.Events
{
  public class BookStoreSetupEvent : IDomainEvent
  {
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public BookStoreSetupEvent(Guid id, string name)
    {
      Id = id;
      Name = name;
    }
  }
}

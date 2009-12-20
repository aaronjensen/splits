using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Impl.Domain.Events;
using Splits.Application;

namespace Bookstore.Application.Impl.Domain
{
  public class BookStore : IEntity
  {
    public static Guid DefaultId = new Guid("c4b2bb7e-10f9-4ee6-913a-3d89e88f27a0");
    readonly Guid _id;
    bool _isSetup;

    public Guid Id
    {
      get { return _id; }
    }

    protected BookStore() {}
    public BookStore(Guid id)
    {
      _id = id;
    }

    public void Setup(string name)
    {
      if (_isSetup) throw new Exception("Bookstore already setup");

      _isSetup = true;
      DomainEvent.Raise(new BookStoreSetupEvent(_id, name));
    }
  }
}

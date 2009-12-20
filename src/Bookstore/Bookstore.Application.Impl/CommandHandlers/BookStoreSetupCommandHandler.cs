using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bookstore.Application.Commands;
using Bookstore.Application.Impl.Domain;
using Splits.Application;

namespace Bookstore.Application.Impl.CommandHandlers
{
  public class BookStoreSetupCommandHandler : ICommandHandler<BookStoreSetupCommand, CommandResult>
  {
    readonly IRepository _repository;

    public BookStoreSetupCommandHandler(IRepository repository)
    {
      _repository = repository;
    }

    public CommandResult Handle(BookStoreSetupCommand command)
    {
      var bookstore = _repository.Get<BookStore>(BookStore.DefaultId);

      if (bookstore == null)
      {
        bookstore = new BookStore(BookStore.DefaultId);
        _repository.Add(bookstore);
      }

      try
      {
        bookstore.Setup(command.Name);
      }
      catch
      {
        return new CommandResult(false);
      }

      return new CommandResult();
    }
  }
}
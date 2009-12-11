using Bookstore.Application.Framework;

namespace Bookstore.Application.Impl.Framework
{
  public interface IQueryHandlerLocator
  {
    IQueryHandler<IQuery<TReport>, TReport> LocateHandler<TReport>(IQuery<TReport> query);
  }
}
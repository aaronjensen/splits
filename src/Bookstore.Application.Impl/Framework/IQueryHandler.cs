using Bookstore.Application.Framework;

namespace Bookstore.Application.Impl.Framework
{
  public interface IQueryHandler<TQuery, TReport>
    where TQuery : IQuery<TReport>
  {
    TReport Handle(TQuery query);
  }
}
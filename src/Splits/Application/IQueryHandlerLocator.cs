using Splits;

namespace Splits.Application
{
  public interface IQueryHandlerLocator
  {
    IQueryHandler<IQuery<TReport>, TReport> LocateHandler<TReport>(IQuery<TReport> query);
  }
}
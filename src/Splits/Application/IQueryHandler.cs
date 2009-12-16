namespace Splits.Application
{
  public interface IQueryHandler<TQuery, TReport>
    where TQuery : IQuery<TReport>
  {
    TReport Handle(TQuery query);
  }
}
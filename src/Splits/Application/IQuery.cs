using System;

namespace Splits.Application
{
  public interface IQuery
  {
    Guid QueryId { get; }
  }

  public interface IQuery<TResult> : IQuery
  {
  }

  public class Query<TResult> : IQuery<TResult>
  {
    public Query()
    {
      QueryId = Guid.NewGuid();
    }

    public Guid QueryId { get; private set; }
  }
}
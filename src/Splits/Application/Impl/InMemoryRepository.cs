using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Splits;

namespace Splits.Application.Impl
{
  public class InMemoryRepository : IRepository
  {
    ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
    IDictionary<Type, IDictionary<Guid, object>> _repository = new Dictionary<Type, IDictionary<Guid, object>>();

    public TEntity Get<TEntity>(Guid id) where TEntity : class, IEntity
    {
      _lock.EnterReadLock();
      try
      {
        if (!_repository.ContainsKey(typeof(TEntity))) return null;

        if (!_repository[typeof(TEntity)].ContainsKey(id)) return null;

        return (TEntity)_repository[typeof(TEntity)][id];
      }
      finally
      {
        _lock.ExitReadLock();
      }
    }

    public void Add<TEntity>(TEntity entity) where TEntity : class, IEntity
    {
      if (entity == null) throw new ArgumentNullException("entity");
      _lock.EnterWriteLock();

      try
      {
        if (!_repository.ContainsKey(typeof(TEntity)))
        {
          _repository[typeof(TEntity)] = new Dictionary<Guid, object>();
        }

        _repository[typeof(TEntity)][entity.Id] = entity;
      }
      finally
      {
        _lock.ExitWriteLock();
      }
    }
  }
}
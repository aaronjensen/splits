﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Splits.Application
{
  public interface IRepository
  {
    TEntity Get<TEntity>(Guid id) where TEntity : class, IEntity;
    void Add<TEntity>(TEntity entity) where TEntity : class, IEntity;
  }
}
using Chinook.Model.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Chinook.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseModel
    {
        void Add(TEntity entity);

        void AddRange(List<TEntity> entity);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        TEntity Get(Expression<Func<TEntity, bool>> predicate = null);

        TEntity Get(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void Delete(int id);

        bool IsExist(Expression<Func<TEntity, bool>> predicate);

    }
}

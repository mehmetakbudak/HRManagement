using Chinook.Storage.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Chinook.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseModel
    {
        Task Add(TEntity entity);

        Task AddRange(List<TEntity> entity);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null);

        IQueryable<TEntity> GetAllNoTracking();

        Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null);

        void Update(TEntity entity);

        Task Delete(TEntity entity);

        Task Delete(int id);

        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
    }
}

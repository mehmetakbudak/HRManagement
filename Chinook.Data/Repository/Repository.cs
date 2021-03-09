using System;
using System.Linq;
using Chinook.Model.Entities;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Chinook.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private bool _isDisposed;

        public Repository(IUnitOfWork unitOfWork) : this(unitOfWork.Context)
        {

        }
        public Repository(ChinookContext context)
        {
            _isDisposed = false;
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("Entity is null");

            _dbSet.Add(entity);
        }

        public virtual void AddRange(List<TEntity> entity)
        {
            _dbSet.AddRange(entity);
        }

        /// <summary>
        /// Verilen Generic Entity' i siler.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Delete(TEntity entity)
        {
            entity = Get(x => x.Id == entity.Id);

            if (entity != null)
                _dbSet.Remove(entity);
        }

        /// <summary>
        /// Verilen Id yi bulup Entity ' i siler.
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(int id)
        {
            var entity = Get(x => x.Id == id);

            if (entity != null)
                _dbSet.Remove(entity);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet.OrderByDescending(x => x.Id).AsQueryable();
        }

        public virtual IQueryable<TEntity> GetAllNoTracking()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include != null)
                query = include(query);

            if (predicate != null)
                return query.FirstOrDefault(predicate);

            return query.FirstOrDefault();
        }

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                return query.FirstOrDefault(predicate);

            return query.FirstOrDefault();
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity is null");

            _dbSet.Update(entity);
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
            _isDisposed = true;

        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (include != null)
                query = include(query);

            if (predicate != null)
                query = query.Where(predicate);

            return query;
        }

        public virtual bool IsExist(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("entity is null");

            return _dbSet.Any(predicate);
        }
    }
}

using Chinook.Storage.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Chinook.Data.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseModel
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private bool _isDisposed;

        public Repository(IUnitOfWork unitOfWork) : this(unitOfWork.Context) { }

        public Repository(ChinookContext context)
        {
            _isDisposed = false;
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRange(List<TEntity> entity)
        {
            await _dbSet.AddRangeAsync(entity);
        }

        public async Task Delete(TEntity entity)
        {
            entity = await Get(x => x.Id == entity.Id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task Delete(int id)
        {
            var entity = await Get(x => x.Id == id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbSet.OrderByDescending(x => x.Id).AsQueryable();
        }

        public IQueryable<TEntity> GetAllNoTracking()
        {
            return _dbSet.AsQueryable().AsNoTracking();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null)
            {
                return await query.FirstOrDefaultAsync(predicate);
            }
            return await query.FirstOrDefaultAsync();
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate = null)
        {
            IQueryable<TEntity> query = _dbSet;
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        #region disposed
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
            _isDisposed = true;
        }
        #endregion
    }
}

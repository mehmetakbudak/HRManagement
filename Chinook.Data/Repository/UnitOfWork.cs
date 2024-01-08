using Chinook.Storage.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Chinook.Data.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly ChinookContext _context;
        private bool _disposed;
        private IDbContextTransaction _objTran;

        public ChinookContext Context
        {
            get { return _context; }
        }

        public UnitOfWork(ChinookContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _objTran.CommitAsync();
        }

        public async Task<IDbContextTransaction> CreateTransaction()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task Rollback()
        {
            await _objTran.RollbackAsync();
            await _objTran.DisposeAsync();
        }

        public async Task<int> SaveChanges()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> Repository<T>() where T : BaseModel
        {
            return new Repository<T>(_context);
        }
    }
}
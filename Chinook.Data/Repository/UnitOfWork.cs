using Chinook.Model.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace Chinook.Data.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly ChinookContext _context;
        private bool _disposed;
        private string _errorMessage = string.Empty;
        private IDbContextTransaction _objTran;

        public ChinookContext Context
        {
            get { return _context; }
        }

        public UnitOfWork(ChinookContext context)
        {
            // dispose edildiği için  NEW TContext() kullanılabilir;
            _context = context;
        }

        public void Commit()
        {
            _objTran.Commit();
        }

        public void CreateTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }

        public void Save()
        {
            try
            {
                _context.SaveChanges();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _context.Dispose();
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
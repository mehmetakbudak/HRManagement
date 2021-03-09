using Chinook.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Data.Repository
{
    public interface IUnitOfWork
    {
        //bakılacak
        ChinookContext Context { get; }
        IRepository<T> Repository<T>() where T : BaseModel;
        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
    }
}

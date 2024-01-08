using Chinook.Storage.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Chinook.Data.Repository
{
    public interface IUnitOfWork
    {
        ChinookContext Context { get; }
        IRepository<T> Repository<T>() where T : BaseModel;
        Task<IDbContextTransaction> CreateTransaction();
        Task Commit();
        Task Rollback();
        Task<int> SaveChanges();
    }
}

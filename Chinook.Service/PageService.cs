using System.Linq;
using Chinook.Storage.Models;
using Chinook.Storage.Entities;
using System.Threading.Tasks;
using Chinook.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace Chinook.Service
{
    public interface IPageService
    {
        IQueryable<Page> Get();
        Task<Page> GetById(int id);
        Task<ServiceResult> Post(PageModel model);
        Task<ServiceResult> Put(PageModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class PageService : IPageService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public IQueryable<Page> Get()
        {
            return _unitOfWork.Repository<Page>()
                .GetAll(x => !x.Deleted)
                .Include(x => x.Menu)
                .AsQueryable();
        }

        public async Task<Page> GetById(int id)
        {
            var page = await _unitOfWork.Repository<Page>().Get(x => x.Id == id);
            return page;
        }

        public Task<ServiceResult> Post(PageModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceResult> Put(PageModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task<ServiceResult> Delete(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}

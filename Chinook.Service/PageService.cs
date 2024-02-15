using Chinook.Data;
using Chinook.Storage.Entities;
using Chinook.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Chinook.Service
{
    public interface IPageService
    {
        IQueryable<PageDmo> Get();
        Task<PageDmo> GetById(int id);
        Task<ServiceResult> Post(PageModel model);
        Task<ServiceResult> Put(PageModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class PageService : IPageService
    {
        private readonly ChinookContext _context;

        public PageService(ChinookContext context)
        {
            _context = context;
        }

        public IQueryable<PageDmo> Get()
        {
            return _context.Pages.Where(x => !x.Deleted).AsQueryable();
        }

        public async Task<PageDmo> GetById(int id)
        {
            var page = await _context.Pages.FirstOrDefaultAsync(x => x.Id == id);
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

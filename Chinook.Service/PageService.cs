using Chinook.Data;
using Chinook.Model.Entities;
using System.Linq;

namespace Chinook.Service
{
    public interface IPageService
    {
        IQueryable<Page> Get();
    }

    public class PageService : IPageService
    {
        private readonly ChinookContext context;
        public PageService(ChinookContext context)
        {
            this.context = context;
        }

        public IQueryable<Page> Get()
        {
            return context.Pages.Where(x => !x.Deleted);
        }
    }
}

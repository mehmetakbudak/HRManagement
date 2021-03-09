using Chinook.Data;
using Chinook.Model.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Chinook.Service
{
    public interface IBlogService
    {
        IQueryable<Blog> GetAll();
        List<Blog> GetAllByCategoryUrl(string categoryUrl);
    }
    public class BlogService : IBlogService
    {
        private readonly ChinookContext context;
        public BlogService(ChinookContext context)
        {
            this.context = context;
        }

        public IQueryable<Blog> GetAll()
        {
            return context.Blogs.Where(x => !x.Deleted);
        }

        public List<Blog> GetAllByCategoryUrl(string categoryUrl)
        {
            var list = context.Blogs.Where(x => !x.Deleted && x.Published && x.BlogCategory.Url == categoryUrl).ToList();
            return list;
        }
    }
}

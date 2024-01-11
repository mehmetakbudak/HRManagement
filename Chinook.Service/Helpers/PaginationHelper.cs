using Chinook.Storage.Models;
using System.Linq;

namespace Chinook.Service.Helpers
{
    public class PaginationHelper<T>
    {
        public static PaginationModel<T> Paginate(IQueryable<T> data, FilterModel model)
        {
            var list = new PaginationModel<T>
            {
                Count = data.Count(),
                Page = model.First,
                PageSize = model.Rows,
                Data = data.Skip(model.First).Take(model.Rows).ToList()
            };
            return list;
        }
    }
}

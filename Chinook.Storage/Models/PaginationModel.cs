using System.Collections.Generic;

namespace Chinook.Storage.Models
{
    public class PaginationModel<T>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public List<T> Data { get; set; }
    }
}


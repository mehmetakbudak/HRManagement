using System;
namespace Chinook.Model.Models
{
    public class FilterModel
    {
        public FilterModel()
        {
            Page = 1;
            PageSize = 5;
        }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}


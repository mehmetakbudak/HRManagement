using System;
using System.Collections.Generic;

namespace Chinook.Model.Models
{
    public class PaginationModel<T>
    {
        public int Count { get; set; }

        public List<T> List { get; set; }
    }
}


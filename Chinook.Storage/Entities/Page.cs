using System;
using System.Collections.Generic;
using System.Text;

namespace Chinook.Storage.Entities
{
    public class Page : BaseModel
    {
        public int? MenuId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public bool Deleted { get; set; }
        public Menu Menu { get; set; }
    }
}

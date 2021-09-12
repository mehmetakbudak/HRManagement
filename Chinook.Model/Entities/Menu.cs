using Chinook.Model.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chinook.Model.Entities
{
    public class Menu : BaseModel
    {
        public string Name { get; set; }
        public MenuType Type { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeletable { get; set; }
        public bool Deleted { get; set; }

        public virtual List<MenuItem> MenuItems { get; set; }
        public virtual List<Page> Pages { get; set; }
    }
}

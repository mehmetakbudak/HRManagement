using Chinook.Storage.Enums;
using System.Collections.Generic;

namespace Chinook.Storage.Entities
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

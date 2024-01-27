using Chinook.Storage.Enums;
using System.Collections.Generic;

namespace Chinook.Storage.Entities
{
    public class MenuDmo : BaseModel
    {
        public string Name { get; set; }

        public MenuType Type { get; set; }

        public bool IsDeletable { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<MenuItemDmo> MenuItems { get; set; }
    }
}

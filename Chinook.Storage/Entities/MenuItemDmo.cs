using System.Collections.Generic;

namespace Chinook.Storage.Entities
{
    public class MenuItemDmo : BaseModel
    {
        public int MenuId { get; set; }

        public MenuDmo Menu { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual List<MenuItemAccessRightDmo> MenuItemAccessRights { get; set; }
    }
}

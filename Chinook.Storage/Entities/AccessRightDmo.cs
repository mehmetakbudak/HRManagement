using System.Collections.Generic;

namespace Chinook.Storage.Entities
{
    public class AccessRightDmo : BaseModel
    {
        public int AccessRightCategoryId { get; set; }

        public AccessRightCategoryDmo AccessRightCategory { get; set; }

        public string Name { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual List<RoleAccessRightDmo> RoleAccessRights { get; set; }

        public virtual List<AccessRightEndpointDmo> AccessRightEndpoints { get; set; }

        public virtual List<MenuItemAccessRightDmo> MenuItemAccessRights { get; set; }
    }
}


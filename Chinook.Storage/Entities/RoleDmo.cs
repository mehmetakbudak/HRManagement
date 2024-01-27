using System.Collections.Generic;

namespace Chinook.Storage.Entities
{
    public class RoleDmo : BaseModel
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }

        public virtual List<RoleAccessRightDmo> RoleAccessRights { get; set; }

        public virtual List<UserRoleDmo> UserRoles { get; set; }
    }
}

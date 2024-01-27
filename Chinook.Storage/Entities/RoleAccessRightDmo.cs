namespace Chinook.Storage.Entities
{
    public class RoleAccessRightDmo : BaseModel
    {
        public int RoleId { get; set; }

        public RoleDmo Role { get; set; }

        public int AccessRightId { get; set; }

        public AccessRightDmo AccessRight { get; set; }
    }
}

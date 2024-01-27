namespace Chinook.Storage.Entities
{
    public class UserRoleDmo : BaseModel
    {
        public int UserId { get; set; }

        public UserDmo User { get; set; }

        public int RoleId { get; set; }

        public RoleDmo Role { get; set; }
    }
}

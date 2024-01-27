namespace Chinook.Storage.Entities
{
    public class UserAccessRightDmo : BaseModel
    {
        public int UserId { get; set; }

        public UserDmo User { get; set; }

        public int AccessRightId { get; set; }

        public AccessRightDmo AccessRight { get; set; }
    }
}


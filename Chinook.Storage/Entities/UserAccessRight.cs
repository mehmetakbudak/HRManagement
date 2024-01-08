using System;
namespace Chinook.Storage.Entities
{
    public class UserAccessRight : BaseModel
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int AccessRightId { get; set; }

        public AccessRight AccessRight { get; set; }
    }
}


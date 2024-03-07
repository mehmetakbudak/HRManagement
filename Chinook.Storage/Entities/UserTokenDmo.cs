using System;

namespace Chinook.Storage.Entities
{
    public class UserTokenDmo : BaseModel
    {
        public int UserId { get; set; }
        
        public UserDmo User { get; set; }

        public string Token { get; set; }

        public DateTime? TokenExpireDate { get; set; }
    }
}

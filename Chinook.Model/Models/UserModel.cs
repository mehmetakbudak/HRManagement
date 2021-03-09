using Chinook.Model.Entities;
using System;
using System.Threading;

namespace Chinook.Model.Models
{
    public class UserModel : BaseModel
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? TitleId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; }
        public string Phone { get; set; }
    }

    public class UserTokenModel
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Token { get; set; }
    }

    public class LoginModel
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class PasswordModel
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ReNewPassword { get; set; }
    }

    public class AuthTokenContent
    {
        public int UserId { get; set; }

        private static readonly AsyncLocal<AuthTokenContent> _current = new AsyncLocal<AuthTokenContent>();

        public static AuthTokenContent Current
        {
            get => _current.Value;
            set => _current.Value = value;
        }
    }
}

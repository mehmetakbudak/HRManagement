using Chinook.Storage.Entities;
using System;
using System.Threading;

namespace Chinook.Storage.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? TitleId { get; set; }
        public int? ProvinceId { get; set; }
        public int? CityId { get; set; }
        public int? ReportedUserId { get; set; }
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

    public class UserGridModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TitleName { get; set; }
        public string ReportedUserName { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; }
        public string Phone { get; set; }
    }
}

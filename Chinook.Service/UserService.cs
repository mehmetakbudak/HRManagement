using Chinook.Data;
using Chinook.Storage.Entities;
using Chinook.Storage.Models;
using Chinook.Service.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Chinook.Service.Exceptions;

namespace Chinook.Service
{
    public interface IUserService
    {
        IQueryable<UserGridModel> Get();
        Task<UserModel> Get(int id);
        Task<UserDmo> GetById(int id);
        Task<ServiceResult> Login(LoginModel model);
        string NewToken(string userId);
        Task<ServiceResult> ChangePassword(PasswordModel model);
        Task<UserModel> GetProfile();
        Task<ServiceResult> UpdateProfile(UserModel model);
        Task<ServiceResult> Put(UserModel model);
        Task<ServiceResult> Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly JwtModel jwt;
        private readonly ChinookContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IOptions<JwtModel> jwt,
            IHttpContextAccessor httpContextAccessor,
            ChinookContext context)
        {
            this.jwt = jwt.Value;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public IQueryable<UserGridModel> Get()
        {
            return _context.Users.Where(x => !x.Deleted)
                .Include(x => x.Title)
                .Include(x => x.City)
                .ThenInclude(x => x.Province)
                .Include(x => x.ReportedUser)
                .AsNoTracking()
                .Select(x => new UserGridModel
                {
                    Address = x.Address,
                    BirthDate = x.BirthDate,
                    CityName = x.City.Name,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    HireDate = x.HireDate,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    LastName = x.LastName,
                    Phone = x.Phone,
                    ProvinceName = x.City.Province.Name,
                    ReportedUserName = $"{x.ReportedUser.FirstName} {x.ReportedUser.LastName}",
                    TitleName = x.Title.Name
                });
        }

        public async Task<UserModel> Get(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(x => !x.Deleted && x.Id == id)
                .Include(x => x.City)
                .AsNoTracking()
                .Select(x => new UserModel
                {
                    Address = x.Address,
                    BirthDate = x.BirthDate,
                    CityId = x.CityId,
                    EmailAddress = x.Email,
                    FirstName = x.FirstName,
                    HireDate = x.HireDate,
                    Id = x.Id,
                    IsActive = x.IsActive,
                    LastName = x.LastName,
                    Phone = x.Phone,
                    ProvinceId = x.City.ProvinceId,
                    TitleId = x.TitleId,
                    ReportedUserId = x.ReportedUserId
                }).FirstOrDefaultAsync();
        }

        public string NewToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwt.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", userId)
                }),
                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public async Task<ServiceResult> Login(LoginModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var loginUser = new UserTokenModel();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.IsActive && !x.Deleted && x.Email == model.EmailAddress && x.Password == model.Password);

            if (user == null)
            {
                throw new NotFoundException("The email address and password are incorrect.");
            }

            bool isCreateToken = false;
            var token = user.Token;

            if (user.TokenExpireDate.HasValue)
            {
                if (!(user.TokenExpireDate >= DateTime.Now))
                {
                    isCreateToken = true;
                }
            }
            else
            {
                isCreateToken = true;
            }
            if (isCreateToken)
            {
                token = NewToken(user.Id.ToString());
                user.Token = token;
                user.TokenExpireDate = DateTime.Now.AddHours(2);

                await _context.SaveChangesAsync();
            }

            loginUser = new UserTokenModel
            {
                EmailAddress = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };
            serviceResult.Data = loginUser;
            return serviceResult;
        }

        public async Task<UserDmo> GetById(int id)
        {
            var user = await _context.Users
                .Include(a => a.City)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
            return user;
        }

        public async Task<ServiceResult> ChangePassword(PasswordModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId && x.Password == model.OldPassword);

            if (user == null)
            {
                throw new NotFoundException("The current password is incorrect.");
            }

            if (model.NewPassword != model.ReNewPassword)
            {
                throw new BadRequestException("Password not matched.");
            }

            user.Password = model.NewPassword;
            await _context.SaveChangesAsync();
            return serviceResult;
        }

        public async Task<UserModel> GetProfile()
        {
            var userId = _httpContextAccessor.HttpContext.User.UserId();
            var user = await GetById(userId);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            var model = new UserModel
            {
                Id = user.Id,
                EmailAddress = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TitleId = user.TitleId,
                ProvinceId = user.City?.ProvinceId,
                CityId = user.CityId,
                Address = user.Address,
                BirthDate = user.BirthDate,
                HireDate = user.HireDate,
                IsActive = user.IsActive,
                Phone = user.Phone
            };
            return model;
        }

        public async Task<ServiceResult> UpdateProfile(UserModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var userId = _httpContextAccessor.HttpContext.User.UserId();

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Id == userId && x.IsActive && !x.Deleted);

            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            user.Address = model.Address;
            user.CityId = model.CityId;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Phone = model.Phone;

            await _context.SaveChangesAsync();

            return serviceResult;
        }

        public async Task<ServiceResult> Put(UserModel model)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.Id && !x.Deleted);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            user.HireDate = model.HireDate;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Phone = model.Phone;
            user.BirthDate = model.BirthDate;
            user.CityId = model.CityId;
            user.IsActive = model.IsActive;
            user.ReportedUserId = model.ReportedUserId;
            user.Address = model.Address;

            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<ServiceResult> Delete(int id)
        {
            var result = new ServiceResult { StatusCode = HttpStatusCode.OK };

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            user.Deleted = true;

            await _context.SaveChangesAsync();

            return result;
        }
    }
}
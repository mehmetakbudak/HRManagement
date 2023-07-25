using Chinook.Data.Repository;
using Chinook.Model.Entities;
using Chinook.Model.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Chinook.Service.Extensions;

namespace Chinook.Service
{
    public interface IUserService
    {

        Task<ServiceResult> Authenticate(LoginModel model);
        string NewToken(string userId);
        Task<User> GetById(int id);
        Task<ServiceResult> ChangePassword(PasswordModel model);
        Task<ServiceResult> UpdateProfile(UserModel model);
    }

    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly JwtModel jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUnitOfWork unitOfWork,
            IOptions<JwtModel> jwt,
            IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWork = unitOfWork;
            this.jwt = jwt.Value;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<ServiceResult> Authenticate(LoginModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            try
            {
                var loginUser = new UserTokenModel();
                var user = await unitOfWork.Repository<User>()
                    .Get(x => x.IsActive && !x.Deleted && x.Email == model.EmailAddress && x.Password == model.Password);

                if (user != null)
                {
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

                        await unitOfWork.SaveChanges();
                    }

                    loginUser = new UserTokenModel
                    {
                        EmailAddress = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Token = token
                    };
                    serviceResult.Data = loginUser;
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Email adresi veya şifre hatalıdır.";
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<User> GetById(int id)
        {
            var user = await unitOfWork.Repository<User>()
                .GetAll(x => x.Id == id && !x.Deleted)
                .Include(a => a.City)
                .FirstOrDefaultAsync();

            return user;
        }

        public async Task<ServiceResult> ChangePassword(PasswordModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var user = await unitOfWork.Repository<User>()
                    .Get(x => x.Id == userId && x.Password == model.OldPassword);

                if (user != null && (model.NewPassword == model.ReNewPassword))
                {
                    user.Password = model.NewPassword;

                    await unitOfWork.SaveChanges();
                }
                else
                {
                    throw new Exception("Şifre değiştirme işlemi gerçekleştirilemedi.");
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }

        public async Task<ServiceResult> UpdateProfile(UserModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var userId = _httpContextAccessor.HttpContext.User.UserId();

                var user = await unitOfWork.Repository<User>()
                    .Get(x => x.Id == userId && x.IsActive && !x.Deleted);

                if (user != null)
                {
                    user.Address = model.Address;
                    user.BirthDate = model.BirthDate ?? model.BirthDate.Value.Date;
                    user.CityId = model.CityId;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Phone = model.Phone;

                    await unitOfWork.SaveChanges();
                }
                else
                {
                    serviceResult.StatusCode = HttpStatusCode.NotFound;
                    serviceResult.Message = "Kullanıcı bulunamadı.";
                }
            }
            catch (Exception ex)
            {
                serviceResult.StatusCode = HttpStatusCode.InternalServerError;
                serviceResult.Message = ex.Message;
            }
            return serviceResult;
        }
    }
}

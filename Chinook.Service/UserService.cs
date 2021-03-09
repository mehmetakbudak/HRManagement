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

namespace Chinook.Service
{
    public interface IUserService
    {
        ServiceResult Authenticate(LoginModel model);
        string NewToken(string userId);
        User GetById(int id);
        ServiceResult ChangePassword(PasswordModel model);
        ServiceResult UpdateProfile(UserModel model);
    }

    public class UserService : IUserService
    {
        readonly IUnitOfWork unitOfWork;
        readonly JwtModel jwt;

        public UserService(
            IUnitOfWork unitOfWork,
            IOptions<JwtModel> jwt)
        {
            this.unitOfWork = unitOfWork;
            this.jwt = jwt.Value;

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

        public ServiceResult Authenticate(LoginModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };

            try
            {
                var loginUser = new UserTokenModel();
                var user = unitOfWork.Repository<User>()
                    .Get(x =>
                    x.IsActive && !x.Deleted &&
                    x.Email == model.EmailAddress &&
                    x.Password == model.Password);

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
                        unitOfWork.Save();
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

        public User GetById(int id)
        {
            var user = unitOfWork.Repository<User>().Get(x => x.Id == id && !x.Deleted,
                x => x.Include(a => a.City));
            return user;
        }

        public ServiceResult ChangePassword(PasswordModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var user = unitOfWork.Repository<User>().Get(x => x.Id == AuthTokenContent.Current.UserId && x.Password == model.OldPassword);

                if (user != null && (model.NewPassword == model.ReNewPassword))
                {
                    user.Password = model.NewPassword;
                    unitOfWork.Save();
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

        public ServiceResult UpdateProfile(UserModel model)
        {
            var serviceResult = new ServiceResult { StatusCode = HttpStatusCode.OK };
            try
            {
                var user = unitOfWork.Repository<User>().Get(x => x.Id == AuthTokenContent.Current.UserId && x.IsActive && !x.Deleted);

                if (user != null)
                {
                    user.Address = model.Address;
                    user.BirthDate = model.BirthDate ?? model.BirthDate.Value.Date;
                    user.CityId = model.CityId;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Phone = model.Phone;
                    unitOfWork.Save();
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

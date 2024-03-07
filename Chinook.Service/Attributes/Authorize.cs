using Chinook.Storage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;

namespace Chinook.Service.Attributes
{
    public class Authorize : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var userTokenService = (IUserTokenService)context.HttpContext.RequestServices.GetService(typeof(IUserTokenService));
                var configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));

                var request = context.HttpContext.Request;
                var token = request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Token bulunamadı.");
                }
                string tokenString = token.Split(' ')[1];
                if (string.IsNullOrEmpty(tokenString))
                {
                    throw new Exception("Token formatı uygun değil.");
                }
                var handler = new JwtSecurityTokenHandler();
                var tokenDescrypt = handler.ReadJwtToken(tokenString);
                var appSettingsSection = configuration.GetSection("Jwt");
                var secret = appSettingsSection.GetSection("Secret").Value;
                var key = Encoding.ASCII.GetBytes(secret);

                if (Int32.TryParse(tokenDescrypt.Payload["UserId"].ToString(), out int userId))
                {
                    try
                    {
                        handler.ValidateToken(tokenString, new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        }, out SecurityToken securityToken);
                    }
                    catch
                    {
                        throw new Exception("Token doğrulanamadı.");
                    }

                    var userToken = await userTokenService.GetByUserId(userId);

                    if (userToken != null)
                    {
                        if (!string.IsNullOrEmpty(userToken.Token))
                        {
                            if (userToken.Token.ToLower() != tokenString.ToLower())
                            {
                                throw new Exception("Token süresi dolmuş. Tekrar giriş yapınız.");
                            }
                        }
                        else
                        {
                            throw new Exception("Lütfen giriş yapınız.");
                        }
                        if (userToken.TokenExpireDate.HasValue)
                        {
                            var tokenStartDate = userToken.TokenExpireDate.Value.AddHours(-2);
                            var tokenEndDate = userToken.TokenExpireDate.Value;

                            if (!((tokenStartDate <= DateTime.Now) && (tokenEndDate >= DateTime.Now)))
                            {
                                throw new Exception("Token süresi dolmuş. Tekrar giriş yapınız.");
                            }
                        }
                        else
                        {
                            throw new Exception("Token süresi dolmuş. Tekrar giriş yapınız.");
                        }
                    }
                    else
                    {
                        context.Result = new NotFoundObjectResult(new ServiceResult
                        {
                            Message = "Kullanıcı bulunamadı.",
                            StatusCode = HttpStatusCode.NotFound
                        });
                    }
                }
                else
                {
                    throw new Exception("Token hatalı.");
                }
            }
            catch (Exception ex)
            {
                context.Result = new UnauthorizedObjectResult(new ServiceResult
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.Unauthorized
                });
            }
        }
    }
}

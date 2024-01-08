using System;
using System.Linq;
using System.Security.Claims;
using Chinook.Storage.Consts;

namespace Chinook.Service.Extensions
{
    public static class ClaimsExtensions
    {
        public static int UserId(this ClaimsPrincipal principal)
        {
            var userIdClaim = principal.Claims.FirstOrDefault(x => x.Type == JwtTokenPayload.UserId);

            if (userIdClaim == null || !Int32.TryParse(userIdClaim.Value, out int userId))
            {
                return 0;
            }

            return userId;
        }
    }
}


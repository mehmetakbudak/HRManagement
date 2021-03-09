using Chinook.Model.Helpers;
using Chinook.Model.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Chinook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginModel model)
        {
            try
            {
                var result = userService.Authenticate(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromBody]PasswordModel model)
        {
            try
            {
                var result = userService.ChangePassword(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}
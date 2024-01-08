using Chinook.Storage.Helpers;
using Chinook.Storage.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Chinook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = _userService.Get();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _userService.Get(id);
            return Ok(result);
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel model)
        {
            try
            {
                var result = await _userService.Authenticate(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordModel model)
        {
            try
            {
                var result = await _userService.ChangePassword(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UserModel model)
        {
            var result = await _userService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }
    }
}
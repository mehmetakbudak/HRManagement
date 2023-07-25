using Chinook.Model.Helpers;
using Chinook.Model.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using Chinook.Service.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Chinook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService userService;

        public ProfileController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var user = await userService.GetById(User.UserId());

            if (user != null)
            {
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
                return Ok(model);
            }
            return null;
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UserModel model)
        {
            try
            {
                var result = await userService.UpdateProfile(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}
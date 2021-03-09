using Chinook.Model.Helpers;
using Chinook.Model.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Chinook.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;

        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var list = menuService.GetAll();
            return Ok(list);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]MenuModel model)
        {
            try
            {
                var result = menuService.Post(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody]MenuModel model)
        {
            try
            {
                var result = menuService.Put(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = menuService.Delete(id);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}
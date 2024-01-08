using System;
using System.Threading.Tasks;
using Chinook.Storage.Helpers;
using Chinook.Storage.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteCategoryController : ControllerBase
    {
        private readonly INoteCategoryService noteCategoryService;

        public NoteCategoryController(
            INoteCategoryService noteCategoryService)
        {
            this.noteCategoryService = noteCategoryService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = noteCategoryService.Get();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NoteCategoryModel model)
        {
            try
            {
                var result = await noteCategoryService.Post(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] NoteCategoryModel model)
        {
            try
            {
                var result = await noteCategoryService.Put(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await noteCategoryService.Delete(id);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}
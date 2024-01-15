using Chinook.Service;
using Chinook.Service.Attributes;
using Chinook.Storage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
            var result = await noteCategoryService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] NoteCategoryModel model)
        {
            var result = await noteCategoryService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await noteCategoryService.Delete(id);
            return Ok(result);
        }
    }
}
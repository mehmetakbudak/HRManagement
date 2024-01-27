using Chinook.Service;
using Chinook.Service.Attributes;
using Chinook.Storage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chinook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogCategoryController : Controller
    {
        IBlogCategoryService blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            this.blogCategoryService = blogCategoryService;
        }

        [Authorize]
        [HttpPost("GetByFilter")]
        public IActionResult GetByFilter([FromBody] BlogCategoryFilterModel model)
        {
            var list = blogCategoryService.GetByFilter(model);
            return Ok(list);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await blogCategoryService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] BlogCategoryModel model)
        {
            var result = await blogCategoryService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] BlogCategoryModel model)
        {
            var result = await blogCategoryService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await blogCategoryService.Delete(id);
            return Ok(result);
        }
    }
}
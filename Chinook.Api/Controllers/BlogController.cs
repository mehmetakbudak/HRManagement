using Chinook.Service;
using Chinook.Service.Attributes;
using Chinook.Storage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chinook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpPost("GetByFilter")]
        [Authorize]
        public IActionResult Get([FromBody] BlogFilterModel model)
        {
            var result = _blogService.GetByFilter(model);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _blogService.GetById(id);
            return Ok(result);

        }

        [HttpGet("GetBlogsByCategoryUrl/{categoryUrl}")]
        public async Task<IActionResult> GetBlogsByCategoryUrl(string categoryUrl)
        {
            var list = await _blogService.GetBlogsByCategoryUrl(categoryUrl);
            return Ok(list);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] BlogModel model)
        {
            var result = _blogService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] BlogModel model)
        {
            var result = _blogService.Put(model);
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var result = _blogService.Delete(id);
            return Ok(result);
        }
    }
}
using Chinook.Storage.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
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
            try
            {
                var result = _blogService.GetByFilter(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _blogService.GetById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
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
            try
            {
                var result = _blogService.Post(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] BlogModel model)
        {
            try
            {
                var result = _blogService.Put(model);
                return Ok(result);
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
                var result = _blogService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });

            }
        }
    }
}

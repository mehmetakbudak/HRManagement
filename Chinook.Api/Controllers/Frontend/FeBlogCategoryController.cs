using Chinook.Model.Models;
using Chinook.Service;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Chinook.Api.Controllers.Frontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeBlogCategoryController : ControllerBase
    {
        private readonly IBlogCategoryService blogCategoryService;

        public FeBlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            this.blogCategoryService = blogCategoryService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            try
            {
                var list = blogCategoryService.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}

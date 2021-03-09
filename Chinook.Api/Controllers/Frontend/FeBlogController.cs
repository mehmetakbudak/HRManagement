using Chinook.Model.Models;
using Chinook.Service;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Chinook.Api.Controllers.Frontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeBlogController : ControllerBase
    {
        private readonly IBlogService blogService;
        public FeBlogController(IBlogService blogService)
        {
            this.blogService = blogService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            try
            {
                var list = blogService.GetAll();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}

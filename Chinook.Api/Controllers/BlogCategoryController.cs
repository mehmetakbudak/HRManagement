using Chinook.Model.Helpers;
using Chinook.Model.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Chinook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogCategoryController : ControllerBase
    {
        IBlogCategoryService blogCategoryService;
        public BlogCategoryController(IBlogCategoryService blogCategoryService)
        {
            this.blogCategoryService = blogCategoryService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                var result = blogCategoryService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]BlogCategoryModel model)
        {
            try
            {
                var result = blogCategoryService.Post(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody]BlogCategoryModel model)
        {
            try
            {
                var result = blogCategoryService.Put(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = blogCategoryService.Delete(id);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}
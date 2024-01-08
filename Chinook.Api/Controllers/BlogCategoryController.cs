using Chinook.Api;
using Chinook.Storage.Helpers;
using Chinook.Storage.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using DevExtreme.AspNet.Data;
using Elfie.Serialization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
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

        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = blogCategoryService.Get();
            return Json(await DataSourceLoader.LoadAsync(list, loadOptions));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] BlogCategoryModel model)
        {
            try
            {
                var result = await blogCategoryService.Post(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] BlogCategoryModel model)
        {
            try
            {
                var result = await blogCategoryService.Put(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await blogCategoryService.Delete(id);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}
using Chinook.Data.Repository;
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
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteCategoryController : ControllerBase
    {
        private readonly INoteCategoryService noteCategoryService;
        public NoteCategoryController(
            INoteCategoryService noteCategoryService,
            IUnitOfWork unitOfWork)
        {
            this.noteCategoryService = noteCategoryService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var list = noteCategoryService.GetAll();
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Post([FromBody]NoteCategoryModel model)
        {
            try
            {
                var result = noteCategoryService.Post(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody]NoteCategoryModel model)
        {
            try
            {
                var result = noteCategoryService.Put(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var result = noteCategoryService.Delete(id);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}
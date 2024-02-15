using Chinook.Service.Attributes;
using Chinook.Storage.Model;
using CMS.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskCategoryController : Controller
    {
        private readonly ITaskCategoryService _taskCategoryService;

        public TaskCategoryController(ITaskCategoryService taskCategoryService)
        {
            _taskCategoryService = taskCategoryService;            
        }       
       
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _taskCategoryService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskCategoryService.GetById(id);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskCategoryModel model)
        {
            var result = await _taskCategoryService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TaskCategoryModel model)
        {
            var result = await _taskCategoryService.Put(model);
            return Ok(result);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskCategoryService.Delete(id);
            return Ok(result);
        }        
    }
}

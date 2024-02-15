using Chinook.Service;
using Chinook.Service.Attributes;
using Chinook.Storage.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Admin.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }       
       
        [HttpGet]        
        public async Task<IActionResult> Get()
        {
            var list = await _taskService.Get();
            return Ok(list);
        }

        [HttpGet("{id}")]        
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _taskService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskModel model)
        {
            var result = await _taskService.Post(model);
            return Ok(result);
        }

        [HttpPut]        
        public async Task<IActionResult> Put([FromBody] TaskModel model)
        {
            var result = await _taskService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]        
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskService.Delete(id);
            return Ok(result);
        }
    }
}

using Chinook.Service;
using Chinook.Service.Attributes;
using Chinook.Storage.Model;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chinook.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskStatusController : ControllerBase
    {
        private readonly ITaskStatusService _taskStatusService;
        public TaskStatusController(ITaskStatusService taskStatusService)
        {
            _taskStatusService = taskStatusService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskStatus()
        {
            var list = await _taskStatusService.GetAll();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskStatus(int id)
        {
            var result = await _taskStatusService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskStatusModel model)
        {
            var result = await _taskStatusService.Post(model);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] TaskStatusModel model)
        {
            var result = await _taskStatusService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _taskStatusService.Delete(id);
            return Ok(result);
        }
    }
}

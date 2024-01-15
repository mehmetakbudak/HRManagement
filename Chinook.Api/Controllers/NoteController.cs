using Chinook.Service;
using Chinook.Service.Attributes;
using Chinook.Storage.Helpers;
using Chinook.Storage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chinook.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController : ControllerBase
    {
        public readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpPost("GetByFilter")]
        public IActionResult GetByFilter([FromBody] NoteFilterModel model)
        {
            var list = _noteService.GetByFilter(model);
            return Ok(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _noteService.GetById(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NoteModel model)
        {
            var result = await _noteService.Post(model);
            return StatusCode((int)result.StatusCode, result.CreateReturnModel());
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] NoteModel model)
        {
            var result = await _noteService.Put(model);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _noteService.Delete(id);
            return Ok(result);
        }
    }
}
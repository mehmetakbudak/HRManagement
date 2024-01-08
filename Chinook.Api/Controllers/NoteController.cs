using Chinook.Storage.Helpers;
using Chinook.Storage.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet]
        public IActionResult Get()
        {
            var list = _noteService.Get();
            return Ok(list);
        }

        [HttpGet("GetByCategoryId/{categoryId:int}")]
        public IActionResult GetByCategoryId(int categoryId)
        {
            var list = _noteService.GetByCategoryId(categoryId);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NoteModel model)
        {
            try
            {
                var result = await _noteService.Post(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] NoteModel model)
        {
            try
            {
                var result = await _noteService.Put(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _noteService.Delete(id);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }

        [HttpPut("move")]
        public async Task<IActionResult> Move([FromBody] NoteModel model)
        {
            try
            {
                var result = await _noteService.Move(model);
                return StatusCode((int)result.StatusCode, result.CreateReturnModel());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ServiceResult { Message = ex.Message });
            }
        }
    }
}
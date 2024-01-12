using Chinook.Storage.Enums;
using Chinook.Storage.Helpers;
using Chinook.Storage.Models;
using Chinook.Service;
using Chinook.Service.Attributes;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chinook.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : Controller
    {
        private readonly IMenuService menuService;

        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var list = menuService.Get();
            return Json(await DataSourceLoader.LoadAsync(list, loadOptions));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var menu = await menuService.GetById(id);
            return Ok(menu);
        }

        [HttpGet("GetFrontendMenu")]
        public async Task<IActionResult> GetFrontendMenu()
        {
            var menu = await menuService.GetFrontendMenu();
            return Ok(menu);
        }

        [HttpGet("GetType/{type}")]
        public async Task<IActionResult> GetType(MenuType type)
        {
            var menu = await menuService.GetByType(type);
            return Ok(menu);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] MenuModel model)
        {
            var result = await menuService.Post(model);
            return StatusCode((int)result.StatusCode, result.CreateReturnModel());
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] MenuModel model)
        {
            var result = await menuService.Put(model);
            return StatusCode((int)result.StatusCode, result.CreateReturnModel());
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await menuService.Delete(id);
            return StatusCode((int)result.StatusCode, result.CreateReturnModel());
        }
    }
}
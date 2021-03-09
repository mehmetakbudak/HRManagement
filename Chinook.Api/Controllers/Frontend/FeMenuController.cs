using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chinook.Model.Enums;
using Chinook.Service;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Api.Controllers.Frontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeMenuController : ControllerBase
    {
        private readonly IMenuService menuService;

        public FeMenuController(IMenuService menuService )
        {
            this.menuService = menuService;
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            var menu = menuService.GetById(id);
            return Ok(menu);
        }       

        [HttpGet("GetType/{type}")]
        public IActionResult GetType(MenuType type)
        {
            var menu = menuService.GetByType(type);
            return Ok(menu);
        }
    }
}

using Chinook.Service;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IPageService pageService;
        public PageController(IPageService pageService)
        {
            this.pageService = pageService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = pageService.Get();
            return Ok(list);
        }
    }
}

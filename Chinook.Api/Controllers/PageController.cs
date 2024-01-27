using Chinook.Service;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chinook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        public PageController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var list = _pageService.Get();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _pageService.GetById(id);
            return Ok(result);
        }
    }
}

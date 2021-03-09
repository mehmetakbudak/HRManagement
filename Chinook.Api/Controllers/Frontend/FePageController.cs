using Chinook.Service;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Api.Controllers.Frontend
{
    [Route("api/[controller]")]
    [ApiController]
    public class FePageController : ControllerBase
    {
        private readonly IPageService pageService;
        public FePageController(IPageService pageService)
        {
            this.pageService = pageService;
        }

        [HttpGet]
        [EnableQuery]
        public IActionResult Get()
        {
            var list = pageService.GetAll();
            return Ok(list);
        }
    }
}

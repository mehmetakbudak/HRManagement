using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Chinook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LookupController : ControllerBase
    {
        private readonly IProvinceService _provinceService;
        private readonly ICityService _cityService;
        private readonly IBlogCategoryService _blogCategoryService;
        private readonly ITitleService _titleService;

        public LookupController(
            IProvinceService provinceService,
            ICityService cityService,
            IBlogCategoryService blogCategoryService,
            ITitleService titleService)
        {
            _provinceService = provinceService;
            _cityService = cityService;
            _blogCategoryService = blogCategoryService;
            _titleService = titleService;
        }

        [Authorize]
        [HttpGet("Titles")]
        public IActionResult Get()
        {
            var list = _titleService.Get();
            return Ok(list);
        }

        [Authorize]
        [HttpGet("Provinces")]
        public IActionResult Provinces()
        {
            var list = _provinceService.Get();
            return Ok(list);
        }

        [Authorize]
        [HttpGet("Cities/{provinceId}")]
        public IActionResult Cities(int provinceId)
        {
            var list = _cityService.GetByProvinceId(provinceId);
            return Ok(list);
        }

        [HttpGet("BlogCategories")]
        public async Task<IActionResult> GetBlogCategories()
        {
            var list = await _blogCategoryService.GetByLookup();
            return Ok(list);
        }
    }
}
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
        private readonly ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [Authorize]
        [HttpGet("Get")]
        public IActionResult Get()
        {
            var list = _lookupService.Get();
            return Ok(list);
        }

        [Authorize]
        [HttpGet("Provinces")]
        public IActionResult Provinces()
        {
            var list = _lookupService.GetProvinces();
            return Ok(list);
        }

        [Authorize]
        [HttpGet("Cities/{provinceId}")]
        public IActionResult Cities(int provinceId)
        {
            var list = _lookupService.GetCitiesByProvinceId(provinceId);
            return Ok(list);
        }

        [HttpGet("BlogCategories")]
        public async Task<IActionResult> GetBlogCategories()
        {
            var list = await _lookupService.GetBlogCategories();
            return Ok(list);
        }
    }
}
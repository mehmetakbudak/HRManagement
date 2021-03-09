using Chinook.Service;
using Chinook.Service.Attributes;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

namespace Chinook.Api.Controllers
{
    public class LookupController : ODataController
    {
        private readonly ILookupService lookupService;

        public LookupController(ILookupService lookupService)
        {
            this.lookupService = lookupService;
        }

        [Authorize]
        [EnableQuery]
        [HttpGet("Get")]
        public IActionResult Get()
        {
            return Ok(lookupService.Get());
        }

        [Authorize]
        [EnableQuery]
        [HttpGet("Provinces")]
        public IActionResult Provinces()
        {
            return Ok(lookupService.GetProvinces());
        }

        [Authorize]
        [EnableQuery]
        [HttpGet("Cities")]
        public IActionResult Cities()
        {
            return Ok(lookupService.GetCities());
        }
    }
}
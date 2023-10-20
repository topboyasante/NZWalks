using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        [HttpGet] 
        public IActionResult GetAllRegions() {
            var regions = new List<Region>()
            {
                new Region
                {
                    Name= "Wellington"
                }
            };
            return Ok(regions);
        }
    }
}

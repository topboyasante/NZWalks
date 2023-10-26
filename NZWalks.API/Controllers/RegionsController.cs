using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet] 
        public async  Task<IActionResult> GetAllRegions() {
            var regions = await regionRepository.getAllRegionsAsync();

            //return DTO regions
            /*var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(region => {
                var regionDTO = new Models.DTO.Region()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Area = region.Area,
                    Code = region.Code,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population,
                };

                regionsDTO.Add(regionDTO);
            });*/


            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionById")]
        public async Task<IActionResult> GetRegionById(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null) { return NotFound(); }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(AddRegionRequest addRegionRequest)
        {
            //Convert the request(DTO) to a domain model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };
            //pass details to the repository
            region = await regionRepository.AddAsync(region);
            //convert data back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population,
            };
            return CreatedAtAction(nameof(GetRegionById),new {id = region.Id},regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get Region from DB
            var region = await regionRepository.DeleteAsync(id); ;

            //If null return NotFound
            if(region == null)
            {
                return NotFound();
            }

            //Convert response to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
            };

            //Return Ok
            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> updateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequest updatedRegion)
        {
            //Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = updatedRegion.Code,
                Area = updatedRegion.Area,
                Lat = updatedRegion.Lat,
                Long = updatedRegion.Long,
                Name = updatedRegion.Name,
                Population = updatedRegion.Population,
            };
            //Update Region using Repo
            region = await regionRepository.updateAsync(id,region);
            //If null then return NotFound
            if(region == null)
            {
                return NotFound();
            }
            //Convert Domain back to DTO
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = updatedRegion.Code,
                Area = updatedRegion.Area,
                Lat = updatedRegion.Lat,
                Long = updatedRegion.Long,
                Name = updatedRegion.Name,
                Population = updatedRegion.Population,
            };
            //Return OK
            return Ok(regionDTO); 
        }
    }
}

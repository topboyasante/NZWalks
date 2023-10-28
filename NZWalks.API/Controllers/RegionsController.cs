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
            //Validate Request
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }

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
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population,
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
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequest updatedRegion)
        {
            //Validate Request
            if (!ValidateUpdateRegionAsync(updatedRegion))
            {
                return BadRequest(ModelState);
            }
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


        #region Private methods
        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest == null)
            {
                ModelState.AddModelError(nameof(addRegionRequest),
                        $"The data fields are empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                        $"{nameof(addRegionRequest.Code)} cannot be null, empty, or a whitespace");
            } 
            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name),
                        $"{nameof(addRegionRequest.Name)} cannot be null, empty, or a whitespace");
            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area),
                        $"{nameof(addRegionRequest.Area)} cannot be less than or equal to zero");
            }
            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat),
                        $"{nameof(addRegionRequest.Lat)} cannot be less than or equal to zero");
            } 
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long),
                        $"{nameof(addRegionRequest.Long)} cannot be less than or equal to zero");
            }
            if (addRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                        $"{nameof(addRegionRequest.Population)} cannot be less than zero");
            }
            if(ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if(updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest),
                        $"The data fields are empty");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                        $"{nameof(updateRegionRequest.Code)} cannot be null, empty, or a whitespace");
            }
            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name),
                        $"{nameof(updateRegionRequest.Name)} cannot be null, empty, or a whitespace");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                        $"{nameof(updateRegionRequest.Area)} cannot be less than or equal to zero");
            }
            if (updateRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Lat),
                        $"{nameof(updateRegionRequest.Lat)} cannot be less than or equal to zero");
            }
            if (updateRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long),
                        $"{nameof(updateRegionRequest.Long)} cannot be less than or equal to zero");
            }
            if (updateRegionRequest.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                        $"{nameof(updateRegionRequest.Population)} cannot be less than zero");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}

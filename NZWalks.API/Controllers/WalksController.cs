using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            var walks = await walkRepository.GetAllWalksAsync();
            //Convert from domain from DTO
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkById")]
        public async Task<IActionResult> GetWalkById(Guid id)
        {
            //Get walk from DB
            var walk = await walkRepository.GetAsync(id);
            //Check if walk exists
            if (walk == null) { return NotFound(); }
            //Convert from domain from DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            //Return specific walk
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to Domain object
            var walkDomain = new Models.Domain.Walk
            {
               Length = addWalkRequest.Length,
               Name = addWalkRequest.Name,
               RegionId = addWalkRequest.RegionId,
               WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };

            //Pass Domain object to repository
            walkDomain = await walkRepository.AddAsync(walkDomain);

            //Convert domain object back to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };

            //Send DTO back to client
            return CreatedAtAction(nameof(GetWalkById), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalksAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            //Convert from DTO to domain
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
            };

            //Pass Domain to Repository
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            //Handle Null response
            if(walkDomain == null)
            {
                return NotFound();
            }

            //Convert Domain Response to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Length = walkDomain.Length,
                Name = walkDomain.Name,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId,
            };

            //return response
            return Ok(walkDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            //Get Region from DB
            var walk = await walkRepository.DeleteAsync(id); ;

            //If null return NotFound
            if (walk == null)
            {
                return NotFound();
            }

            //Convert response to DTO
            var walkDTO = new Models.DTO.Walk
            {
                Id = walk.Id,
                Length = walk.Length,
                Name = walk.Name,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId,
            };

            //Return Ok
            return Ok(walkDTO);
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalksDifficultyRepository walksDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalksDifficultyRepository walksDifficultyRepository, IMapper mapper)
        {
            this.walksDifficultyRepository = walksDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalksAysnc()
        {
            var walkDifficulties = await walksDifficultyRepository.GetAllWalksAsync();
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);
            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyById")]
        public async Task<IActionResult> GetWalkDifficultyById(Guid id)
        {
            //Get walk from DB
            var walkDifficulty = await walksDifficultyRepository.GetAsync(id);
            //Check if walk exists
            if (walkDifficulty == null) { return NotFound(); }
            //Convert from domain from DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            //Return specific walk
            return Ok(walkDifficultyDTO);
        }


        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Convert DTO to Domain object
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code,
            };

            //Pass Domain object to repository
            walkDifficultyDomain = await walksDifficultyRepository.AddAsync(walkDifficultyDomain);

            //Convert domain object back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficultyDomain.Code,
            };

            //Send DTO back to client
            return CreatedAtAction(nameof(GetWalkDifficultyById), new { id = walkDifficultyDTO.Id }, walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> updateRegionAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Convert DTO to Domain Model
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,
               
            };
            //Update Region using Repo
            walkDifficulty = await walksDifficultyRepository.UpdateAsync(id, walkDifficulty);
            //If null then return NotFound
            if (walkDifficulty == null)
            {
                return NotFound();
            }
            //Convert Domain back to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code,
            };
            //Return OK
            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get Region from DB
            var walkDifficulty = await walksDifficultyRepository.DeleteAsync(id); ;

            //If null return NotFound
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            //Convert response to DTO
            var walkDifficultyDTO = new Models.DTO.WalkDifficulty
            {
                Code = walkDifficulty.Code,
            };

            //Return Ok
            return Ok(walkDifficultyDTO);
        }
    }
}

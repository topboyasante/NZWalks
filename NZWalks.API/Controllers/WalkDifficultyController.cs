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
            //Validate Request
            if (!ValidateAddWalkDifficulty(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
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
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Validate Request
            if (!ValidateUpdateWalkDifficulty(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }
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

        #region Private Methods
            private bool ValidateAddWalkDifficulty(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
            {
                if (addWalkDifficultyRequest == null)
                {
                    ModelState.AddModelError(nameof(addWalkDifficultyRequest),
                            $"The data fields are empty");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
                {
                    ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),
                            $"{nameof(addWalkDifficultyRequest.Code)} cannot be null, empty, or a whitespace");
                }
                if (ModelState.ErrorCount > 0)
                {
                    return false;
                }
                return true;
            }

            private bool ValidateUpdateWalkDifficulty(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
            {
                if (updateWalkDifficultyRequest == null)
                {
                    ModelState.AddModelError(nameof(updateWalkDifficultyRequest),
                            $"The data fields are empty");
                    return false;
                }
                if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
                {
                    ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code),
                            $"{nameof(updateWalkDifficultyRequest.Code)} cannot be null, empty, or a whitespace");
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

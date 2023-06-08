using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var walkDifficulties = await walkDifficultyRepository.GetAllAsync();
            var walkDiffsDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);

            return Ok(walkDiffsDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDiff = await walkDifficultyRepository.GetAsync(id);

            var walkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiff);

            return Ok(walkDiffDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Models.DTO.AddWalkDifficultyRequest walkDiffRequest)
        {

            //if (!ValidateAddAsync(walkDiffRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            var walkDiff = new Models.Domain.WalkDifficulty() 
            {
                Code = walkDiffRequest.Code
            };

            walkDiff = await walkDifficultyRepository.AddAsync(walkDiff);

            var walkDiffDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDiff.Id,
                Code = walkDiff.Code
            };

            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new {id = walkDiffDTO.Id}, walkDiffDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDiff = await walkDifficultyRepository.DeleteAsync(id);

            if (walkDiff == null)
            {
                return NotFound();
            }

            var walkDiffDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDiff);

            return Ok(walkDiffDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkDifficultyRequest updateWalkDiffReq)
        {

            //if (!ValidateUpdateWalkDifficultyAsync(updateWalkDiffReq))
            //{
            //    return BadRequest(ModelState);
            //}

            var walkDiff = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDiffReq.Code,
            };

            walkDiff  = await walkDifficultyRepository.UpdateAsync(id, walkDiff);

            if (walkDiff == null)
            {
                return NotFound();
            }

            var walkDiffDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDiff.Id,
                Code = walkDiff.Code,
            };

            return Ok(walkDiffDTO);
        }

        #region Private Methods

        private bool ValidateAddAsync(Models.DTO.AddWalkDifficultyRequest walkDiffRequest)
        {
            if (walkDiffRequest == null)
            {
                ModelState.AddModelError(nameof(walkDiffRequest), 
                    $"{nameof(walkDiffRequest)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(walkDiffRequest.Code))
            {
                ModelState.AddModelError(nameof(walkDiffRequest.Code),
                    $"{nameof(walkDiffRequest.Code)} cannot be null or empty or white space.");
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficultyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDiffReq)
        {

            if (updateWalkDiffReq == null)
            {
                ModelState.AddModelError(nameof(updateWalkDiffReq),
                    $"{nameof(updateWalkDiffReq)} cannot be empty.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDiffReq.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDiffReq.Code),
                    $"{nameof(updateWalkDiffReq.Code)} cannot be null or empty or white space.");
                return false;
            }

            return true;
        }

        #endregion
    }
}

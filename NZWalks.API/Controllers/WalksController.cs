using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            Mapper = mapper;
            WalkRepository = walkRepository;
        }

        public IMapper Mapper { get; }
        public IWalkRepository WalkRepository { get; }

        // CREATEa walk
        // POST: /api/walks

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDTO addWalkRequestDTO)

        {
            //Map DTO to domain model
            var walkDomainModel = Mapper.Map<Walk>(addWalkRequestDTO);
            await WalkRepository.CreateAsync(walkDomainModel);
            //Map domain model to DTO
            return Ok(Mapper.Map<WalkDto>(walkDomainModel));

        }

        // Get all walks
        //GET: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomainModel = await WalkRepository.GetAllAsync();
            //Map Domain model to DTO
            return Ok(Mapper.Map<List<WalkDto>>(walksDomainModel));

        }

        // Get Walk by id
        // GET: /api/walks/{id}

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await WalkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            //Map Domain model to DTO
            return Ok(Mapper.Map<WalkDto>(walkDomainModel));
        }



        //Update walk by  id
        // PUT: /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            //Map DTO to domain model
            var walkDomainModel = Mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await WalkRepository.UpdateAsync(id, walkDomainModel);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map domain model to DTO
            return Ok(Mapper.Map<WalkDto>(walkDomainModel));
        }

        // Delete walk By Id
        //DELETE: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await WalkRepository.DeleteAsync(id);
            if (deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            //Map domain Model to DTO
            return Ok(Mapper.Map<WalkDto>(deletedWalkDomainModel));
            
        }
    }
}

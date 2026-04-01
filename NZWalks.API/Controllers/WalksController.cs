using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domains;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        public WalksController(IMapper mapper, IWalkRepository walkRepository, NZWalksDbContext dbcontext)
        {
            Mapper = mapper;
            WalkRepository = walkRepository;
            Dbcontext = dbcontext;
        }

        public IMapper Mapper { get; }
        public IWalkRepository WalkRepository { get; }
        public NZWalksDbContext Dbcontext { get; }

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
        //GET: /api/walks?filterOn=Name&filterQuery=Track&filterQuery&sortBy=Name&isAscending=true&pageNumber=1&pageSize=5
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string?filterQuery,
          [FromQuery] string? sortBy, [FromQuery] bool?isAscending,
          [FromQuery]int pageNumber = 1, [FromQuery] int pageSize = 5 )
        {
            var totalRecords = await Dbcontext.Walks.CountAsync();

            var walksDomainModel = await WalkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending??true,pageNumber,pageSize);
            //Map Domain model to DTO
            //return Ok(Mapper.Map<List<WalkDto>>(walksDomainModel));
            return Ok(new
            {
                Data = Mapper.Map<List<WalkDto>>(walksDomainModel),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
            });

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

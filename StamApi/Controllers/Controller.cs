using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Domain.Models;
using Core.DomainServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StamApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    [ApiConventionType(typeof(DefaultApiConventions))]
    public abstract class Controller<T, D> : ControllerBase where T : Entity
    {
        private readonly IIdentityRepository _identityRepository;
        private readonly IRepository<T> _repository;
        private readonly IMapper _mapper;

        public Controller(IRepository<T> repository, IIdentityRepository identityRepository,
            IMapper mapper)
        {
            _repository = repository;
            _identityRepository = identityRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public ActionResult<IEnumerable<D>> Get()
        {
            var items = _repository.Get();

            var itemDtos = items.Select(t => _mapper.Map<T, D>(t)).ToList();

            return Ok(itemDtos);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<T>> Get(int id)
        {
            var item = await _repository.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            var itemDto = _mapper.Map<T, D>(item);

            return Ok(itemDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<T>> Post([FromBody] D Dto)
        {
            var userId = User.Claims.First(u => u.Type == ClaimTypes.Sid).Value;
            var currentUser = await _identityRepository.GetUserById(userId);

            var patient = _mapper.Map<D, T>(Dto);

            var createdItem = await _repository.Add(patient);

            var createdItemDto = _mapper.Map<T, D>(createdItem);

            return Ok(createdItemDto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Put(int id, [FromBody] D dto)
        {
            var item = await _repository.Get(id);

            if (item == null)
            {
                return NotFound();
            }
            

            _mapper.Map(dto, item);

            item.Id = id;

            var updatedItem = await _repository.Update(item);

            var itemDto = _mapper.Map<T, D>(updatedItem);

            return Ok(itemDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.Claims.First(u => u.Type == ClaimTypes.Sid).Value;

            await _repository.Delete(id);

            return NoContent();
        }
    }
}
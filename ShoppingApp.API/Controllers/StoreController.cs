using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.API.Data;
using ShoppingApp.API.DTOS;
using ShoppingApp.API.Models;

namespace ShoppingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private readonly IShoppingRepository _repository;
        private readonly IMapper _mapper;
        public StoresController(IShoppingRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var storeFromRepo = await _repository.GetStores();
            if (storeFromRepo == null)
                return NotFound();
            var storeToReturn = _mapper.Map<IEnumerable<StoreToCreateDto>>(storeFromRepo);
            return Ok(storeToReturn);
        }

        [HttpGet("{id}", Name="GetStore")]
        public async Task<IActionResult> Get(int id)
        {
            var storeFromRepo = await _repository.GetStore(id);
            if (storeFromRepo == null)
                return NotFound();
            var storeToReturn = _mapper.Map<StoreToCreateDto>(storeFromRepo);
            return Ok(storeToReturn);
        }



        [HttpPost]
        public async Task<IActionResult> CreateStore([FromBody] StoreToCreateDto storeToCreateDto)
        {

            var storeToCreate = _mapper.Map<Store>(storeToCreateDto);
             if (await _repository.EntityExists(storeToCreate))
                return BadRequest("Store with the same name and location exists");
            _repository.Add(storeToCreate);
            if (await _repository.SaveAllChangesAsync())
            {
                return CreatedAtRoute("GetStore", new {id = storeToCreate.Id}, storeToCreate);
            }
            return BadRequest("An Error occurred while creating the store");
            
        }

    }
}
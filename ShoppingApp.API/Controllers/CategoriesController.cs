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
    public class CategoriesController : ControllerBase
    {
        private readonly IShoppingRepository _repository;
        private readonly IMapper _mapper;
        public CategoriesController(IShoppingRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categoryFromRepo = await _repository.GetCategories();
            if (categoryFromRepo == null)
                return NotFound();
            return Ok(categoryFromRepo);
        }
        [HttpGet("{id}", Name="GetCategory")]
        public async Task<IActionResult> Get(int id)
        {
            var categoryFromRepo = await _repository.GetCategory(id);
            if (categoryFromRepo == null)
                return NotFound();
            return Ok(categoryFromRepo);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryToCreateDto categoryToCreateDto)
        {
            var categoryToCreate = _mapper.Map<Category>(categoryToCreateDto);

            if (await _repository.EntityExists(categoryToCreate))
                return BadRequest("Category Name Exists");
            _repository.Add(categoryToCreate);
            if (await _repository.SaveAllChangesAsync())
            {
                return CreatedAtRoute("GetMerchant", new {id = categoryToCreate.Id}, categoryToCreate);
            }
            return BadRequest("An Error occurred while creating the Category");
            
        }

    }
}
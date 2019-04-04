using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.API.Data;
using ShoppingApp.API.DTOS;
using ShoppingApp.API.Helpers;
using ShoppingApp.API.Models;

namespace ShoppingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly IShoppingRepository _repository;
        private readonly IMapper _mapper;
        public BrandsController(IShoppingRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> GetBrands([FromQuery] QueryParams queryParams)
        {
            var brandFromRepo = await _repository.GetBrands(queryParams);
            var brandToReturn = _mapper.Map<IEnumerable<BrandToReturnDto>>(brandFromRepo);
            return Ok(brandToReturn);
        }
        [HttpGet("{id}", Name="GetBrand")]
        public async Task<IActionResult> Get(int id)
        {
            var brandFromRepo = await _repository.GetBrand(id);
            if (brandFromRepo == null)
                return NotFound();
            return Ok(brandFromRepo);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] BrandToCreateDto brandToCreateDto)
        {
            var categoryFromRepo = await _repository.GetCategory(brandToCreateDto.CategoryId);
            var brandToCreate = _mapper.Map<Brand>(brandToCreateDto);
            if (await _repository.EntityExists(brandToCreate))
                return BadRequest("Brand Name Exists");
            _repository.Add(brandToCreate);
            if (await _repository.SaveAllChangesAsync())
            {
                var brandToReturn = _mapper.Map<BrandToCreateDto>(brandToCreateDto);
                return CreatedAtRoute("GetBrand", new {id = brandToCreate.Id}, brandToReturn);
            }
            return BadRequest("An Error occurred while creating the brand");
            
        }

    }
}
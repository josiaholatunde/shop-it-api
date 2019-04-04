using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ProductsController : ControllerBase
    {
        private readonly IShoppingRepository _repository;
        private readonly IMapper _mapper;

        public ProductsController(IShoppingRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] QueryParams queryParams)
        {
            var products = await _repository.GetProducts(queryParams);
            var productsToReturn = _mapper.Map<IEnumerable<ProductToReturnDto>>(products);
            return Ok(productsToReturn);
        }

        // GET api/values/5
        [HttpGet("{code}", Name="GetProduct")]
        public async Task<IActionResult> Get(string code)
        {
            var productFromRepo = await _repository.GetProduct(code);
            if (productFromRepo == null)
                return NotFound();
            var productToReturn = _mapper.Map<ProductToReturnDto>(productFromRepo);
            return Ok(productToReturn);
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductToCreateDto productToCreateDto)
        {
            var productToCreate = _mapper.Map<Product>(productToCreateDto);
             if (await _repository.EntityExists(productToCreate))
                return BadRequest("Product With Same Name and Vendor Exists");
                var category = await _repository.GetCategory(productToCreate.CategoryId);
            if (category == null)
                return BadRequest("Invalid Category Id");
            var productCode = await _repository.GenerateProductCode(productToCreateDto.CategoryId);
            if (productToCreateDto.Price == 0) 
                return BadRequest("Invalid Price");
            var merchant = await _repository.GetMerchant(productToCreateDto.MerchantId);
            if (merchant == null)
                return BadRequest("The Merchant for whom this product is to be created does not exist");
            var brand = await _repository.GetBrand(productToCreateDto.BrandId);
            if (brand == null)
                return BadRequest("The Brand for whom this product is to be created does not exist");
             var store = await _repository.GetStore(productToCreateDto.StoreId);
            if (store == null)
                return BadRequest("The Store for which this product is to be stored does not exist");
            productToCreate.Code = productCode;
            store.Products.Add(productToCreate);
            if (await _repository.SaveAllChangesAsync())
            {
                var productToReturn = _mapper.Map<ProductToReturnDto>(productToCreate);
                return CreatedAtRoute("GetProduct", new { code = productToReturn.Code}, productToReturn);
            }
            return  BadRequest("An Error occurred while creating product");
        }

        [HttpPost("categories/{categoryId}/brands/{brandId}")]
        public async Task<IActionResult> CreateBrandCategory(int categoryId, int brandId)
        {
            var category = await _repository.GetCategory(categoryId);
             if (category == null)
                return BadRequest("Invalid Category Id");
             var brand = await _repository.GetBrand(brandId);
             if (brand == null)
                return BadRequest("Invalid Brand Id");
            var brandCat = new BrandCategory {
                CategoryId = categoryId,
                BrandId = brandId
            };
            if (await _repository.EntityExists(brandCat))
                return BadRequest("The Brand Id already belongs to this category");
            _repository.Add(brandCat);
            if (await _repository.SaveAllChangesAsync())
            {
                return Ok("Successfully created brand Category");
            }
            return BadRequest("An Error occurred while creating brand category");
        }

        // PUT api/values/5
        [HttpPut("{code}")]
        public async Task<IActionResult> Put(string code, [FromBody] ProductToCreateDto productToUpdateDto)
        {
            if (code != productToUpdateDto.Code)
                return BadRequest(" The id of the product to update does not corresppnd woth that on the route of the request");
            var productFromRepo = await _repository.GetProduct(productToUpdateDto.Code);
            if (productFromRepo == null)
                return NotFound();
            var productToUpdate = _mapper.Map<ProductToCreateDto ,Product>(productToUpdateDto, productFromRepo);
            _repository.Update(productToUpdate);
            if (await _repository.SaveAllChangesAsync())
            {
                var productToReturn = _mapper.Map<ProductToReturnDto>(productToUpdate);
                return NoContent();
            }
            return BadRequest($"An Error occurred while trying to update product with code {productToUpdate.Code}");
        }

        // DELETE api/values/5
        [HttpDelete("{code}")]
        public async Task<IActionResult> Delete(string code)
        {
            var productFromRepo = await _repository.GetProduct(code);
            if (productFromRepo == null)
                return NotFound();
            _repository.Delete(productFromRepo);
            if (await _repository.SaveAllChangesAsync())
                return Ok();
            return BadRequest($"An Error occurred while trying to delete product with code {productFromRepo.Code}");
        }
    }
}

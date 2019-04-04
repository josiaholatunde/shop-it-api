using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.API.Helpers;
using ShoppingApp.API.Models;

namespace ShoppingApp.API.Data
{
    public class ShoppingRepository : IShoppingRepository
    {
        private readonly DataContext _context;

        public ShoppingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
        

        public async Task<string> GenerateProductCode(int categoryId)
        {
            var category = await this.GetCategory(categoryId);
            string prefix = category.Name.ToLower().Substring(0, 3);
            if (await _context.Products.AnyAsync(p => p.CategoryId == categoryId))
            {
                int count = _context.Products.Count(p => p.CategoryId == categoryId);
                //get first three characters of the category
                return $"{prefix}-00{++count}";
            }
            return $"{prefix}-001";
        }

        public async Task<Product> GetProduct(string code)
        {
            var product = await _context.Products
                                        .Include(p => p.Merchant)
                                        .Include(p => p.Features)
                                        .Include(p => p.Brand)
                                        .FirstOrDefaultAsync(p => p.Code == code);
            if (product == null)
                return null;
            return product; 
        }
        public async Task<IEnumerable<Product>> GetProducts(QueryParams queryParams)
        {
            var productList = new List<Product>();
            var products = _context.Products
                                        .Where(p => p.CategoryId == queryParams.CategoryId.Value)
                                        .Include(p => p.Merchant)
                                        .Include(p => p.Features)
                                        .Include(p => p.Brand)
                                        .Include(p => p.CategoryName)
                                        .AsQueryable();
            if (!String.IsNullOrWhiteSpace(queryParams.Price)) 
            {
                var price = queryParams.Price.Split("-");
                string minPrice = price[0];
                string maxPrice = price[1];
                if (maxPrice != "max") 
                {
                    products = products.Where(p => p.Price >= Double.Parse(minPrice) && p.Price <= Double.Parse(maxPrice));
                } else {
                    products = products.Where(p => p.Price >= Double.Parse(minPrice));
                }
            }
            if (!String.IsNullOrWhiteSpace(queryParams.BrandIds))
            {
                var brandIds = queryParams.BrandIds.Split(",");
                
                    products = products.Where(p => ReturnValidBrandIdStatus(p.BrandId, brandIds));

            }
            return await products.ToListAsync();
        }

            private bool ReturnValidBrandIdStatus(int brandId, string[] v)
            {
                for (int i = 0; i < v.Length; i++)
                {
                    if (brandId == int.Parse(v[i]))
                    return true;
                }
                return false;
            }

        

        public async Task<IEnumerable<Merchant>> GetMerchants()
        {
            var merchants = await _context.Merchants.ToListAsync();
            if (merchants == null)
                return null;
            return merchants; 
        }
        public async Task<Merchant> GetMerchant(int id)
        {
            var customer = await _context.Merchants.FirstOrDefaultAsync(p => p.Id == id);
            if (customer == null)
                return null;
            return customer; 
        }
        public async Task<Category> GetCategory(int categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(p => p.Id == categoryId);
            if (category == null)
                return null;
            return category; 
        }
        public async Task<IEnumerable<Category>> GetCategories()
        {
            var category = await _context.Categories
                                        .Take(6)
                                        .ToListAsync();
            if (category == null)
                return null;
            return category; 
        }
        public async Task<IEnumerable<Store>> GetStores()
        {
            var stores = await _context.Stores.ToListAsync();
            if (stores == null)
                return null;
            return stores; 
        }
        public async Task<Store> GetStore(int storeId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(p => p.Id == storeId);
            if (store == null)
                return null;
            return store; 
        }

        public async Task<bool> SaveAllChangesAsync()
        {
            if (await _context.SaveChangesAsync() > 0) 
                return true;
            return false;
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<IEnumerable<BrandCategory>> GetBrands(QueryParams queryParams)
        {
            if (queryParams.CategoryId.HasValue)
            {
                var brands = await _context.BrandCategory
                                            .Where(p => p.CategoryId == queryParams.CategoryId.Value)
                                            .Include(p => p.Brand)
                                            .ToListAsync();
                return brands;
            } 
           return null;
        }
        public async Task<Brand> GetBrand(int brandId)
        {
            var brand = await _context.Brands.FirstOrDefaultAsync(p => p.Id == brandId);
            if (brand == null)
                return null;
            return brand; 
        }

        public async Task<bool> BrandExists(string brandName)
        {
            if (await _context.Brands.AnyAsync(b => b.Name == brandName))
                return true;
            return false;
        }
        public async Task<bool> EntityExists<T>(T entityName) where T : class
        {
            if (entityName is Product)
            {
                Product product = entityName as Product;
                if (await _context.Products.AnyAsync(b => b.Name == product.Name && b.StoreId == product.StoreId))
                    return true;
                return false;
            } else if (entityName is Merchant)
            {
                Merchant customer = entityName as Merchant;
                if (await _context.Merchants.AnyAsync(b => b.Name == customer.Name))
                    return true;
                return false;
            } else if (entityName is Store)
            {
                Store store = entityName as Store;
                if (await _context.Stores.AnyAsync(b => b.Name == store.Name && b.Location == store.Location))
                    return true;
                return false;
            } else if (entityName is Category)
            {
                Category category = entityName as Category;
                if (await _context.Categories.AnyAsync(b => b.Name == category.Name))
                    return true;
                return false;
            } else if (entityName is BrandCategory)
            {
                BrandCategory category = entityName as BrandCategory;
                if (await _context.BrandCategory.AnyAsync(b => b.CategoryId == category.CategoryId && b.BrandId == category.BrandId))
                    return true;
                return false;
            }
            else {
                return false;
            }
            
        }
    }
}
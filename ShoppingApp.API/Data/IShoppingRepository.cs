using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.API.Helpers;
using ShoppingApp.API.Models;

namespace ShoppingApp.API.Data
{
    public interface IShoppingRepository
    {
         Task<string> GenerateProductCode(int categoryId);
         void Add<T>(T entity) where T : class;
         void Delete<T>(T entity) where T : class;
         void Update<T>(T entity) where T : class;
         Task<bool> SaveAllChangesAsync();
         Task<Product> GetProduct(string code);
         Task<Category> GetCategory(int id);
         Task<Merchant> GetMerchant(int id);
         Task<Store> GetStore(int storeId);
         Task<Brand> GetBrand(int brandId);
         Task<bool> EntityExists<T>(T entityName) where T: class;
         Task<IEnumerable<Store>> GetStores();
         Task<IEnumerable<Merchant>> GetMerchants();
         Task<IEnumerable<Product>> GetProducts(QueryParams queryParams);
         Task<IEnumerable<Category>> GetCategories();
         Task<IEnumerable<BrandCategory>> GetBrands(QueryParams queryParams);
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShoppingApp.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BrandCategory> Brands { get; set; }
        public ICollection<Product> Products { get; set; }
        public Category()
        {
            this.Brands = new Collection<BrandCategory>();
            this.Products = new Collection<Product>();
        }
    }
    
}
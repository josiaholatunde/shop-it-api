using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShoppingApp.API.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<BrandCategory> Category { get; set; } 
        public ICollection<Product> Products { get; set; }
        public Brand()
        {
            Category = new Collection<BrandCategory>();
            Products = new Collection<Product>();
        }
    }
}
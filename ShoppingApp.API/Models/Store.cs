using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ShoppingApp.API.Models
{
    public class Store
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ICollection<Product> Products { get; set; }
        public Store()
        {
           Products = new Collection<Product>(); 
        }

    }
    
}
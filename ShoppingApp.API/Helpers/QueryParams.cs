using ShoppingApp.API.Models;

namespace ShoppingApp.API.Helpers
{
    public class QueryParams
    {
        public int? CategoryId { get; set; }
        public string BrandIds { get; set; }
        public string Price { get; set; }
    }
    
}
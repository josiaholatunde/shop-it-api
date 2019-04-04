using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.API.Models
{
    public class Merchant
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
    
}
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.API.DTOS
{
    public class MerchantToCreateDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
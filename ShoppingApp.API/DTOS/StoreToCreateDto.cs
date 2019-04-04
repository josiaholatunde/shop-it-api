using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.API.DTOS
{
    public class StoreToCreateDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
    }
}
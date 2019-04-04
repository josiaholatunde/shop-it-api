using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.API.DTOS
{
    public class CategoryToCreateDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
       /*  [Required]
        public int CategoryId { get; set; } */
    }
}
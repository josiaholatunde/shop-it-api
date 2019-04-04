namespace ShoppingApp.API.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsRecommended { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
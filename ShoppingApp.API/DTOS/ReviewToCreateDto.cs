namespace ShoppingApp.API.DTOS
{
    public class ReviewToCreateDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsRecommended { get; set; }
    }
}
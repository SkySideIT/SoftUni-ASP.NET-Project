namespace TechWorld.Web.ViewModels
{
    public class CartViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}

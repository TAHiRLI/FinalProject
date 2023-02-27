using Medlab.Core.Entities;

namespace MedlabApi.Dtos.ProductReviewDtos
{
    public class ProductReviewGetDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Rate { get; set; }
        public AppUserInProductReviewGetDto AppUser { get; set; }
        public string AppUserId { get; set; }
        public bool IsApproved { get; set; }
        public int ProductId { get; set; }
        public string Link { get; set; }
        public ProductInProductReviewGetDto Product { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
    
    }
    public class AppUserInProductReviewGetDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class ProductInProductReviewGetDto
    {
        public string Name { get; set; }
        public string AvgRating { get;set; }
    }
}

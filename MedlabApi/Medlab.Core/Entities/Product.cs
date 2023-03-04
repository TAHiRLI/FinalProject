using Medlab.Core.Interfaces;

namespace Medlab.Core.Entities
{
    public class Product : BaseEntity, ITrackable
    {
        public string Name { get; set; }
        public string? Desc { get; set; }
        public int AvgRating { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public decimal  DiscoutPercent { get; set; }
        public bool IsFeatured { get; set; }
        public bool StockStatus { get; set; }
        public bool IsSoldIndividual { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(4);
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow.AddHours(4);


        public int? ProductCategoryId { get; set; }
        public ProductCategory? ProductCategory { get; set; }
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public List<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
        public List<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();    
        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}

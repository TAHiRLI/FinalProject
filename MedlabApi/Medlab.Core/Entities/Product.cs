using Medlab.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Medlab.Core.Entities
{
    public class Product : BaseEntity, ITrackable
    {
        public string Name { get; set; }
        public string? Desc { get; set; }
        public int AvgRating { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int DiscoutPercent { get; set; }
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
    }
}

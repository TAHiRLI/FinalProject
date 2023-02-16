using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Product> SimilarProducts { get; set; } = new List<Product>();
        public List<ProductReview> ProductReviews = new List<ProductReview>();
        public ProductReviewViewModel ProductReviewViewModel  { get; set; }
        public bool IsUserReviewed { get; set; } = false;
    }
}

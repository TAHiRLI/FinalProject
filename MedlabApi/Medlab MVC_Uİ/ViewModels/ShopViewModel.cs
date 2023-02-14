using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ShopViewModel
    {
        public List<ProductCategory> Categories { get; set; } = new List<ProductCategory>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Product> FeaturedProducts { get; set; } = new List<Product>();
    }
}

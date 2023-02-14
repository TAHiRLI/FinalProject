using Medlab.Core.Entities;

namespace Medlab_MVC_Uİ.ViewModels
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Product> SimilarProducts { get; set; } = new List<Product>();   
    }
}

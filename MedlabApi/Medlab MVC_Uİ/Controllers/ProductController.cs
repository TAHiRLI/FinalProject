using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Medlab_MVC_Uİ.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            this._productRepository = productRepository;
        }
        public async Task<IActionResult>  Details(int id)
        {
            var product = await _productRepository.GetAsync(x => x.Id == id, "ProductCategory", "ProductImages");
            if (product == null)
                return NotFound();


            ProductDetailsViewModel model = new ProductDetailsViewModel
            {
                Product = product, 
                SimilarProducts = _productRepository.GetAll(x=> x.ProductCategoryId == product.ProductCategoryId, "ProductImages").Take(20).ToList()
            };
            return View(model);
        }
    }
}

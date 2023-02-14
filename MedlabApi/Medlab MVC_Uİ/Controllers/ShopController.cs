using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Medlab_MVC_Uİ.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IProductRepository _productRepository;

        public ShopController(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository)
        {
            this._productCategoryRepository = productCategoryRepository;
            this._productRepository = productRepository;
        }
        public IActionResult Index()
        {
            ShopViewModel model = new ShopViewModel();

            model.Products = _productRepository.GetAll(x => true, "ProductImages").ToList();
            model.Categories = _productCategoryRepository.GetAll(x => x.Products.Count > 0, "Products").ToList();
            model.FeaturedProducts = _productRepository.GetAll(x => x.IsFeatured == true, "ProductImages").OrderByDescending(x=> x.CreatedAt).Take(3).ToList();

            return View(model);
        }
    }
}

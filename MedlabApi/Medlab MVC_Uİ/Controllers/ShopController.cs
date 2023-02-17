using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.EntityFrameworkCore.Query.Internal;

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
        public IActionResult Index(
            string? search = null,
            List<int>? CategoryIds = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            string sort = "AZ"
            )
        {
            var query = _productRepository.GetAll(x => true, "ProductImages");
            if (search != null)
            {
                query = query.Where(x => x.Name.Contains(search));
            }
            if (CategoryIds?.Count > 0)
            {
                query = query.Where(x => CategoryIds.Contains(x.ProductCategoryId ?? 0));
            }
            if (minPrice != null && maxPrice != null)
            {
                query = query.Where(x => x.SalePrice * (100 - x.DiscoutPercent) / 100 >= minPrice && x.SalePrice * (100 - x.DiscoutPercent) / 100 <= maxPrice);
            }
            query = query.OrderBy(x => x.StockStatus == true);
            switch (sort)
            {
                case "HighToLow":
                    query = query.OrderByDescending(x => x.SalePrice * (100 - x.DiscoutPercent) / 100);    
                    break;
                case "LowToHigh":
                    query = query.OrderBy(x => x.SalePrice * (100 - x.DiscoutPercent) / 100);
                    break;
                case "ZA":
                    query = query.OrderByDescending(x => x.Name);
                    break;
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }


            ShopViewModel model = new ShopViewModel();

            model.Products = query.ToList();
            model.Categories = _productCategoryRepository.GetAll(x => x.Products.Count > 0, "Products").ToList();
            model.FeaturedProducts = _productRepository.GetAll(x => x.IsFeatured == true, "ProductImages").OrderByDescending(x => x.CreatedAt).Take(3).ToList();






            decimal min = _productRepository.GetAll(x => true).Min(x => x.SalePrice * (100 - x.DiscoutPercent) / 100);
            decimal max = _productRepository.GetAll(x => true).Max(x => x.SalePrice * (100 - x.DiscoutPercent) / 100);


            ViewBag.minPrice = min;
            ViewBag.maxPrice = max;

            ViewBag.selectedMin = minPrice ?? min;
            ViewBag.selectedMax = maxPrice ?? max;
            ViewBag.CategoryIds = CategoryIds;
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            return View(model);
        }
    }
}

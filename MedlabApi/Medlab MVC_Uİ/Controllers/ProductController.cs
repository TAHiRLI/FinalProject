using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Medlab_MVC_Uİ.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductRepository _productRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductReviewRepository _productReviewRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, UserManager<AppUser> userManager, IProductReviewRepository productReviewRepository, IMapper mapper)
        {
            this._productRepository = productRepository;
            _userManager = userManager;
            _productReviewRepository = productReviewRepository;
            _mapper = mapper;
        }




        //======================
        // 1 Details
        //======================
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetProductForDetails(id);
            if (product == null)
                return NotFound();

            AppUser? user = null;

            if(User.Identity.IsAuthenticated){
             user = await _userManager.FindByNameAsync(User.Identity.Name);

            }



            ProductReviewViewModel ReviewVm = new ProductReviewViewModel();
            if (user != null)
            {
                ReviewVm.UserName = user.UserName;
                ReviewVm.ImageUrl = user.ImageUrl;
            }
            ReviewVm.ProductId = product.Id;



            ProductDetailsViewModel model = new ProductDetailsViewModel
            {
                Product = product,
                SimilarProducts = _productRepository.GetAll(x => x.ProductCategoryId == product.ProductCategoryId, "ProductImages").Take(20).ToList(),
                ProductReviewViewModel = ReviewVm,

            };


            // check if user has commented to show input
            // get the comments depending on the user
            if (user != null)
            {
                model.IsUserReviewed = product.ProductReviews.Any(x => x.AppUserId == user.Id);
                model.ProductReviews = _productReviewRepository.GetAll(x => x.ProductId == product.Id &&  x.IsApproved == true || x.ProductId == product.Id && x.AppUserId == user.Id , "AppUser").OrderByDescending(x => x.CreatedAt).ToList();

            }
            else
            {
                model.ProductReviews = _productReviewRepository.GetAll(x => x.ProductId == product.Id && x.IsApproved == true, "AppUser").OrderByDescending(x => x.CreatedAt).ToList();
            }


            return View(model);
        }



        //======================
        // 2 Review Product
        //======================


        [HttpPost]
        [Authorize(Roles = "Member")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReviewProduct(ProductReviewViewModel ReviewVm)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            var product = await _productRepository.GetProductForDetails(ReviewVm.ProductId);
            if (product == null)
                return NotFound();


            ReviewVm.ImageUrl = user.ImageUrl;
            ReviewVm.UserName = user.UserName;
            ReviewVm.ProductId = product.Id;

            ProductDetailsViewModel model = new ProductDetailsViewModel
            {
                Product = product,
                SimilarProducts = _productRepository.GetAll(x => x.ProductCategoryId == product.ProductCategoryId, "ProductImages").Take(20).ToList(),
                ProductReviewViewModel = ReviewVm,
                ProductReviews = _productReviewRepository.GetAll(x => x.ProductId == product.Id && x.IsApproved == true || x.ProductId == product.Id && x.AppUserId == user.Id, "AppUser").OrderByDescending(x => x.CreatedAt).ToList()
            };

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            ProductReview productReview = new ProductReview
            {
                AppUser = user,
                Rate = ReviewVm.Rate,
                Text = ReviewVm.Text
            };

            product.ProductReviews.Add(productReview);
            product.AvgRating = (int)Math.Ceiling(product.ProductReviews.Average(x => x.Rate));

            _productRepository.Commit();

            return RedirectToAction("Details", new { id=product.Id});
        }


        //======================
        // 3 Get Reviews / Load More
        //======================

        public async Task<IActionResult>  GetReviews(int id, int count = 3, int skipCount = 3)
        {
            List<ProductReview> productReviews = new List<ProductReview>();
            if (User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return NotFound();

                 productReviews = _productReviewRepository
               .GetAll(x => x.ProductId == id && x.IsApproved == true || x.ProductId == id && x.AppUserId == user.Id, "AppUser")
               .Skip(skipCount)
               .OrderByDescending(x => x.CreatedAt)
               .Take(count)
               .ToList();
                return PartialView("_ReviewsPartial", productReviews);

            }
            else
            {
                productReviews = _productReviewRepository
               .GetAll(x => x.ProductId == id && x.IsApproved == true, "AppUser")
               .Skip(skipCount)
               .OrderByDescending(x => x.CreatedAt)
               .Take(count)
               .ToList();
            }
           
            return PartialView("_ReviewsPartial", productReviews);
        }
    }
}

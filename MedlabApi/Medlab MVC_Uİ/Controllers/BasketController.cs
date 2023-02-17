using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Medlab_MVC_Uİ.Controllers
{
    public class BasketController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IBasketItemRepository _basketItemRepository;

        public BasketController(IProductRepository productRepository, UserManager<AppUser> userManager, IMapper mapper, IBasketItemRepository basketItemRepository)
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _mapper = mapper;
            _basketItemRepository = basketItemRepository;
        }
        public async Task<IActionResult>  Index()
        {
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            BasketViewModel BasketVm = new BasketViewModel();

            if(user != null)
            {
                var basketItems = _basketItemRepository.GetBasketItemsWithProduct().Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in basketItems)
                {
                    BasketItemViewModel BasketItemVm = _mapper.Map<BasketItemViewModel>(item);
                    BasketVm.BasketItems.Add(BasketItemVm);
                    BasketVm.Total += item.Count * (item.Product.SalePrice * (100 - item.Product.DiscoutPercent) / 100);
                    BasketVm.SubTotal += item.Count * item.Product.SalePrice;
                }
            }
            else
            {
                List<BasketCookieViewModel> BasketCookieItems = new List<BasketCookieViewModel>();

                var basket = HttpContext.Request.Cookies["Basket"];
                if (basket != null)
                    BasketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieViewModel>>(basket);

                foreach (var item in BasketCookieItems)
                {
                    Product cookieProdut = await _productRepository.GetAsync(x => x.Id == item.ProductId, "ProductImages");

                    BasketItemViewModel basketItemViewModel = new BasketItemViewModel
                    {
                        Product = cookieProdut,
                        Count = item.Count,
                    };

                    BasketVm.BasketItems.Add(basketItemViewModel);
                    BasketVm.Total += item.Count * (cookieProdut.SalePrice * (100 - cookieProdut.DiscoutPercent) / 100);
                    BasketVm.SubTotal += item.Count * cookieProdut.SalePrice;
                }
            }


            return View(BasketVm);
        }


        //=======================
        // Add to Basket
        //=======================

        public async Task<IActionResult> AddToBasket(int id, int count = 1)
        {
         

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            var product = await _productRepository.GetAsync(x => x.Id == id && x.StockStatus, "ProductImages");
            if (product == null)
                return NotFound();

            BasketViewModel BasketVm = new BasketViewModel();

            if (user != null)
            {
                // if Exists increment count
                BasketItem basketItem = await _basketItemRepository.GetAsync(x => x.AppUserId == user.Id && x.ProductId == product.Id);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        ProductId = product.Id,
                        Count = count,

                    };

                    await _basketItemRepository.AddAsync(basketItem);
                }
                else
                {
                    basketItem.Count += count;
                }
                _basketItemRepository.Commit();


                // Now map it to basketItemView model
                var basketItems = _basketItemRepository.GetBasketItemsWithProduct().Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in basketItems)
                {
                    BasketItemViewModel BasketItemVm = _mapper.Map<BasketItemViewModel>(item);
                    BasketVm.BasketItems.Add(BasketItemVm);
                    BasketVm.Total += item.Count * (item.Product.SalePrice * (100 - product.DiscoutPercent) / 100);
                    BasketVm.SubTotal += item.Count * item.Product.SalePrice;
                }
            }
            else
            {
                List<BasketCookieViewModel> BasketCookieItems = new List<BasketCookieViewModel>();
                var basket = HttpContext.Request.Cookies["Basket"];
                if (basket != null)
                    BasketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieViewModel>>(basket);

                var basketItem = BasketCookieItems.FirstOrDefault(x => x.ProductId == id);
                if (basketItem != null)
                    basketItem.Count += count;
                else
                {
                    basketItem = new BasketCookieViewModel();
                    basketItem.Count = count;
                    basketItem.ProductId = id;
                    BasketCookieItems.Add(basketItem);
                }

                HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(BasketCookieItems));

                foreach (var item in BasketCookieItems)
                {
                    Product cookieProdut = await _productRepository.GetAsync(x => x.Id == item.ProductId, "ProductImages");

                    BasketItemViewModel basketItemViewModel = new BasketItemViewModel
                    {
                        Product = cookieProdut,
                        Count = item.Count,
                    };

                    BasketVm.BasketItems.Add(basketItemViewModel);
                    BasketVm.Total += item.Count * (cookieProdut.SalePrice * (100 - cookieProdut.DiscoutPercent) / 100);
                    BasketVm.SubTotal += item.Count * cookieProdut.SalePrice;
                }
            }

            return PartialView("_BasketMiniPartial", BasketVm);

        }

        public async Task<IActionResult> RemoveFromBasket(int id)
        {
            BasketViewModel BasketVm = new BasketViewModel();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            if(user != null)
            {
                var existBasketiItem = await _basketItemRepository.GetAsync(x => x.ProductId == id);
                if (existBasketiItem == null)
                    return NotFound();

                _basketItemRepository.Delete(existBasketiItem);
                _basketItemRepository.Commit();

                //now form the basket vm

            }
        }
    }
}

using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace Medlab_MVC_Uİ.Controllers
{
    public class BasketController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IBasketItemRepository _basketItemRepository;
        private readonly IOrderRepository _orderRepository;

        public BasketController(
            IProductRepository productRepository, 
            UserManager<AppUser> userManager,
            IMapper mapper, 
            IBasketItemRepository basketItemRepository,
            IOrderRepository orderRepository
            
            )
        {
            _productRepository = productRepository;
            _userManager = userManager;
            _mapper = mapper;
            _basketItemRepository = basketItemRepository;
            _orderRepository = orderRepository;
        }


        //=======================
        // Index View
        //=======================
        public async Task<IActionResult> Index()
        {
            AppUser? user = null;
            if (User.Identity.IsAuthenticated)
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            BasketViewModel BasketVm = new BasketViewModel();

            if (user != null)
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

        //=======================
        // RemoveFromBasket
        //=======================
        public async Task<IActionResult> RemoveFromBasket(int id)
        {
            BasketViewModel BasketVm = new BasketViewModel();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (user != null)
            {
                var existBasketiItem = await _basketItemRepository.GetAsync(x => x.ProductId == id);
                if (existBasketiItem == null)
                    return NotFound();

                _basketItemRepository.Delete(existBasketiItem);
                _basketItemRepository.Commit();

                //now form the basket vm

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
                var basketItem = BasketCookieItems.FirstOrDefault(x => x.ProductId == id);
                if (basketItem != null)
                    BasketCookieItems.Remove(basketItem);

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


        //=======================
        // Get Basket Totat, SubTotal, Count
        //=======================

        public async Task<ObjectResult> GetBasketInfo()
        {
            AppUser? user = null;
            if (User.Identity.IsAuthenticated)
                user = await _userManager.FindByNameAsync(User.Identity.Name);

            BasketViewModel BasketVm = new BasketViewModel();

            if (user != null)
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
                    Product cookieProdut = await _productRepository.GetAsync(x => x.Id == item.ProductId);

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

            BasketInfoViewModel BasketInfo = new BasketInfoViewModel
            {
                Total = BasketVm.Total,
                Subtotal = BasketVm.SubTotal,
                Count = BasketVm.BasketItems.Count()
            };
            return Ok(BasketInfo);
        }


        //=======================
        // Order form view
        //=======================
        public async Task<IActionResult> Order()
        {
            OrderViewModel model = new OrderViewModel();
            if(User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return NotFound();
                model.Fullname = user.Fullname;
                model.Email = user.Email;
                model.PhoneNumber = user.PhoneNumber;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrderViewModel OrderVm)
        {
            if (!ModelState.IsValid)
                return View(OrderVm);


            Order NewOrder = new Order
            {
                Fullname = OrderVm.Fullname,
                Email = OrderVm.Email,
                PhoneNumber = OrderVm.PhoneNumber,
                ZipCode = OrderVm.ZipCode,
                Address1 = OrderVm.Address1,
                Address2 = OrderVm.Address2,
                Note = OrderVm.Note,
                OrderStatus = null

            };


            if(User.Identity.IsAuthenticated && User.IsInRole("Member"))
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user == null)
                    return NotFound();

                NewOrder.AppUserId = user.Id;
                NewOrder.Email = user.Email;
                NewOrder.PhoneNumber = user.PhoneNumber;

                List<BasketItem> basketItems = _basketItemRepository.GetAll(x => x.AppUserId == user.Id, "Product").ToList();

                foreach (var item in basketItems)
                {
                    OrderItem orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Name = item.Product.Name,
                        SalePrice = item.Product.SalePrice,
                        CostPrice = item.Product.CostPrice,
                        DiscountPercent = item.Product.DiscoutPercent,
                        Count = item.Count
                    };
                    NewOrder.OrderItems.Add(orderItem);
                    _basketItemRepository.Delete(item);

                }
                
                _basketItemRepository.Commit();
            }
            else
            {
                List<BasketCookieViewModel> BasketCookieItems = new List<BasketCookieViewModel>();

                var basket = HttpContext.Request.Cookies["Basket"];
                if (basket != null)
                    BasketCookieItems = JsonConvert.DeserializeObject<List<BasketCookieViewModel>>(basket);

                foreach (var item in BasketCookieItems)
                {
                    Product cookieProduct = await _productRepository.GetAsync(x => x.Id == item.ProductId);

                    if(cookieProduct!= null)
                    {

                        OrderItem orderItem = new OrderItem
                        {
                            ProductId = cookieProduct.Id,
                            Name = cookieProduct.Name,
                            SalePrice = cookieProduct.SalePrice,
                            CostPrice = cookieProduct.CostPrice,
                            DiscountPercent = cookieProduct.DiscoutPercent,
                            Count = item.Count
                        };
                        NewOrder.OrderItems.Add(orderItem);
                    }
                }

                BasketCookieItems = new List<BasketCookieViewModel>();

                HttpContext.Response.Cookies.Append("Basket", JsonConvert.SerializeObject(BasketCookieItems));

            }
            await _orderRepository.AddAsync(NewOrder);

            await _orderRepository.CommitAsync();
            
            return  RedirectToAction("index" , "Home");
        }
    }
}


using AutoMapper;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.Repositories;
using Medlab_MVC_Uİ.ViewModels;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Medlab_MVC_Uİ.Services
{
    public class LayoutService
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IBasketItemRepository _basketItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IMapper _mapper;
        private readonly IBlogRepostiory _blogRepostiory;

        public LayoutService(IHttpContextAccessor httpContext,IMapper mapper, IBlogRepostiory blogRepostiory, ISettingRepository settingRepository, IBasketItemRepository basketItemRepository, IProductRepository productRepository)
        {
            this._settingRepository = settingRepository;
            _basketItemRepository = basketItemRepository;
            _productRepository = productRepository;
            _httpAccessor = httpContext;
            _mapper = mapper;
            _blogRepostiory = blogRepostiory;
        }

        public Dictionary<string,string> GetSetting()
        {
          return  _settingRepository.GetSettingDictionary();
        }
        public async Task<BasketViewModel>  GetBasket()
        {
            BasketViewModel BasketVm = new BasketViewModel();


            if (_httpAccessor.HttpContext.User.Identity.IsAuthenticated && _httpAccessor.HttpContext.User.IsInRole("Member"))
            {
                var userId = _httpAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                var basketItems = _basketItemRepository.GetBasketItemsWithProduct().Where(x => x.AppUserId == userId).ToList();

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

                var basket = _httpAccessor.HttpContext.Request.Cookies["Basket"];
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

            return BasketVm;
        }

        public List<Blog> GetRecentBlogs()
        {
            return _blogRepostiory.GetAll(x => true).OrderByDescending(x => x.CreatedAt).Take(3).ToList();
        }
    }
}

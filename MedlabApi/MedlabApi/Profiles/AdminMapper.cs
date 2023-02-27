using AutoMapper;
using Medlab.Core.Entities;
using MedlabApi.Dtos.BlogDtos;
using MedlabApi.Dtos.ProductCategoryDtos;
using MedlabApi.Dtos.ProductReviewDtos;
using MedlabApi.Dtos.SettingDtos;

namespace MedlabApi.Profiles
{
    public class AdminMapper:Profile
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public AdminMapper(IHttpContextAccessor httpAccessor, IWebHostEnvironment env, IConfiguration config)
        {
            _httpAccessor = httpAccessor;
            _env = env;
            _config = config;

            // Saved for later usage, not imortant
            //var mvcProjectDirectory = new DirectoryInfo(Path.Combine(env.ContentRootPath, "..", "Medlab MVC_Uİ"));
            //var imagePath = Path.Combine(mvcProjectDirectory.FullName, "wwwroot", "Assets");

            CreateMap<Setting, SettingGetDto>();

            // Product
            CreateMap<ProductCategory, ProductCategoryGetDto>();
            CreateMap<ProductCategory, ProductCategoryPostDto >();
            CreateMap<ProductReview, ProductReviewGetDto>()
                .ForMember(x=> x.Link, f=> f.MapFrom(x=> $"{config.GetSection("Mvc:Path").Value}Product/Details/{x.Id}"));
            CreateMap<AppUser, AppUserInProductReviewGetDto>();
            CreateMap<Product, ProductInProductReviewGetDto>();
            //Blog
            CreateMap<Blog, BlogGetDto>()
                .ForMember(x => x.ImageUrl, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Blogs/{x.ImageUrl}"))
                .ForMember(x => x.Link, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Blog/Details/{x.Id}"));
        }
    }
}

using AutoMapper;
using Medlab.Core.Entities;
using MedlabApi.Dtos.BlogDtos;
using MedlabApi.Dtos.DepartmentDtos;
using MedlabApi.Dtos.DoctorDtos;
using MedlabApi.Dtos.ProductCategoryDtos;
using MedlabApi.Dtos.ProductDtos;
using MedlabApi.Dtos.ProductReviewDtos;
using MedlabApi.Dtos.SettingDtos;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Runtime.CompilerServices;

namespace MedlabApi.Profiles
{
    public class AdminMapper : Profile
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

            //===================
            // Product
            //===================

            // category
            CreateMap<ProductCategory, ProductCategoryGetDto>();
            CreateMap<ProductCategory, ProductCategoryPostDto>();

            // Review
            CreateMap<AppUser, AppUserInProductReviewGetDto>();
            CreateMap<Product, ProductInProductReviewGetDto>();
            CreateMap<ProductReview, ProductReviewGetDto>()
                .ForMember(x => x.Link, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Product/Details/{x.Id}"));

            // Product 
            CreateMap<ProductCategory, ProductCategoryInProductListItemDto>();
            CreateMap<ProductCategory, ProductCategoryInProductGetDto>();
            CreateMap<ProductPostDto, Product>();
            CreateMap<Product, ProductGetDto>();
            CreateMap<ProductImage, ProductImageInProductGetDto>()
                .ForMember(x => x.ImageUrl, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Products/{x.ImageUrl}"));

            CreateMap<Product, ProductListItemDto>()
             .ForMember(x => x.ImageUrl, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Products/{x.ProductImages.FirstOrDefault(x => x.IsMain).ImageUrl}"));

            // Blog
            CreateMap<Blog, BlogGetDto>()
                .ForMember(x => x.ImageUrl, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Blogs/{x.ImageUrl}"))
                .ForMember(x => x.Link, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Blog/Details/{x.Id}"));


            // Departments
            CreateMap<Department, DepartmentGetDto>();
            CreateMap<Department, DepartmentPostDto>().ReverseMap();

            // Doctors
            // ---- list item --
            CreateMap<Doctor, DoctorListItemDto>()
                .ForMember(x => x.ImageUrl, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Doctors/{x.ImageUrl}"))
                .ForMember(x => x.Link, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Doctor/Details/{x.Id}"))
                .ForMember(x => x.BlogsCount, f => f.MapFrom(x => x.Blogs.Count));
            CreateMap<Department, DepartmentInDoctorListItemDto>();
            CreateMap<AppUser, AppUserInDoctorListItemDto>();
            // ---- Get Dto ---------
            CreateMap<Doctor, DoctorGetDto>()
                 .ForMember(x => x.ImageUrl, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Doctors/{x.ImageUrl}"))
                 .ForMember(x => x.Link, f => f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Doctor/Details/{x.Id}"))
                .ForMember(x => x.BlogsCount, f => f.MapFrom(x => x.Blogs.Count));
            CreateMap<Department, DepartmentInDoctorGetDto>();
            CreateMap<AppUser, AppUserInDoctorGetDto>();

        }
    }
}

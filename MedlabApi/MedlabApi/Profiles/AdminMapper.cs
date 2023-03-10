using AutoMapper;
using Medlab.Core.Entities;
using MedlabApi.Dtos.AmenityImageDtos;
using MedlabApi.Dtos.BlgoCategoriyDtos;
using MedlabApi.Dtos.BlogDtos;
using MedlabApi.Dtos.DepartmentDtos;
using MedlabApi.Dtos.DoctorDtos;
using MedlabApi.Dtos.MessageDtos;
using MedlabApi.Dtos.OrderDtos;
using MedlabApi.Dtos.ProductCategoryDtos;
using MedlabApi.Dtos.ProductDtos;
using MedlabApi.Dtos.ProductReviewDtos;
using MedlabApi.Dtos.SettingDtos;
using MedlabApi.Dtos.SliderDtos;
using MedlabApi.Dtos.UserDtos;
using MedlabApi.Dtos.ValueDtos;
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
            CreateMap<ProductCategoryPostDto, ProductCategory>().ReverseMap();

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
            // Blog Categories
            CreateMap<BlogCategory, BlogCategoryGetDto>();

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

            // Orders 
            CreateMap<Order, OrderGetDto>();
            CreateMap<AppUser, AppUserInOrderGetDto>();
            CreateMap<OrderItem, OrderItemInOrderGetDto>();
            CreateMap<Product, ProductInOrderItem>()
                .ForMember(x=> x.Link , f=> f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Product/Details/{x.Id}"))
                .ForMember(x=> x.ImageUrl, f=> f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Products/{x.ProductImages.FirstOrDefault(x => x.IsMain).ImageUrl}"));

            // Users
            CreateMap<AppUser, UserGetDto>()
                .ForMember(x=> x.ImageUrl, f=> f.MapFrom(x=> x.IsAdmin==null? $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Doctors/{x.ImageUrl}" : $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Users/{x.ImageUrl}"));

            // Messages
            CreateMap<ContactMessage, MessageGetDto>();
            CreateMap<AppUser, AppUserInMessageGetDto>();
            // Sliders
            CreateMap<Slider, SliderGetDto>()
                .ForMember(x=> x.ImageUrl, f=> f.MapFrom(x=> $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/Sliders/{x.ImageUrl}"));
            CreateMap<SliderPostDto, Slider>();

            // AmenityImages
            CreateMap<AmenityImage, AmenityImageGetDto>()
                .ForMember(x=> x.ImageUrl, f=> f.MapFrom(x => $"{config.GetSection("Mvc:Path").Value}Assets/Uploads/AmenityImages/{x.ImageUrl}"));

            // Values
            CreateMap<Value, ValueGetDto>();
            CreateMap<ValuePostDto, Value>();
            CreateMap<ValuePutDto, Value>();    
        }
    }
}

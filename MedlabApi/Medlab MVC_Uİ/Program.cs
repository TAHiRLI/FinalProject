using AutoMapper;
using FluentValidation.AspNetCore;
using Medlab.Core.Entities;
using Medlab.Core.Repositories;
using Medlab.Data.DAL;
using Medlab.Data.Repositories;
using Medlab_MVC_Uİ.Services;
using Medlab_MVC_Uİ.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using Medlab_MVC_Uİ.Hubs;
using Microsoft.Build.Framework;



//--------------------------
//         Content
//--------------------------
// Fluent Validation
// 1 Database
// 2 Identity
// 3 Google Auth
// 4 Custom Services
// 5 Mapper
// 6 SignalR









var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//===================
// Fluent Validation
//===================
builder.Services.AddControllersWithViews()
   .AddJsonOptions(options =>
   {
       options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
   })
      .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)

    .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<LoginVmValidator>());



//===================
// 1 Database
//===================


builder.Services.AddDbContext<MedlabDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});



//===================
// 2 Identity
//===================


builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 8;
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = true;

}).AddDefaultTokenProviders().AddEntityFrameworkStores<MedlabDbContext>();


//===================
// 3 Google auth
//===================
builder.Services.AddAuthentication()
                   .AddFacebook(options =>
                   {
                       options.AppId = "1552827238569987";
                       options.AppSecret = "82a47b6dd3d803510f2c580d728a803e";
                       options.Scope.Add("email");
                   })
               .AddGoogle(options =>
               {
                   options.ClientId = "233219007455-ga7paq7j1l8e8uq2h6d8ndfjupd505fj.apps.googleusercontent.com";
                   options.ClientSecret = "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";
                   options.SignInScheme = IdentityConstants.ExternalScheme;
               });

//===================
// 4 Custom Services
//===================

builder.Services.AddScoped<LayoutService>();

//General
builder.Services.AddScoped<IValueRepository, ValueRepository>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<ISubscriptionRepostiory, SubscriptionRepository>();
builder.Services.AddScoped<IAmenityImageRepository, AmenityImageRepository>();
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();
//Doctor
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDoctorAppointmentRepository, DoctorAppointmentRepository>();

//Blog
builder.Services.AddScoped<IBlogRepostiory, BlogRepository>();
builder.Services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();

//Product
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductTagRepository, ProductTagRepository>();
builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();

// Order
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped < IOrderItemRepository, OrderItemRepository>();

//======================
// 5 Mapper
//======================

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//======================
// 6 SignalR
//======================
builder.Services.AddSignalR();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapHub<MeetingHub>("/MeetingHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


// Notes for frequent usage 

// Cli Commands
// cd Medlab.Data
// dotnet ef  --startup-project ..\MedlabApi migrations  add 
// dotnet ef  --startup-project ..\MedlabApi database update

//var obj = JsonConvert.SerializeObject(r, Formatting.None,
//            new JsonSerializerSettings()
//            {
//                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//            });

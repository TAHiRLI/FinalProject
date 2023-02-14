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



//--------------------------
//         Content
//--------------------------
// 1 Database
// 2 Identity
// 3 Google Auth
// 4 Custom Services
// 5 Fluent Validation








var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(opt =>
                          opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
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
    

}).AddDefaultTokenProviders().AddEntityFrameworkStores<MedlabDbContext>();


//===================
// 3 Google auth
//===================
builder.Services.AddAuthentication()
               .AddGoogle(options =>
               {
                   options.ClientId = "452097516219-f1gkr0obf8bq4jqhgngp58mea3meedah.apps.googleusercontent.com";
                   options.ClientSecret = "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";
                   options.SignInScheme = IdentityConstants.ExternalScheme;
               });

//===================
// 4 Custom Services
//===================

builder.Services.AddScoped<LayoutService>();

//General
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IValueRepository, ValueRepository>();
builder.Services.AddScoped<IAmenityImageRepository, AmenityImageRepository>();

//Doctor
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

//Blog
builder.Services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
builder.Services.AddScoped<IBlogRepostiory, BlogRepository>();

//Product
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IProductTagRepository, ProductTagRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


//===================
// 5 Fluent Validation
//===================

//builder.Services.AddValidatorsFromAssemblyContaining<LoginVmValidator>();

//builder.Services.AddScoped<IValidator<LoginViewModel>, LoginVmValidator>();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();




// Cli Commands
// cd Medlab.Data
// dotnet ef  --startup-project ..\MedlabApi migrations  add 
// dotnet ef  --startup-project ..\MedlabApi database update
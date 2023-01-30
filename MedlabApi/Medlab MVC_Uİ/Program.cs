using Medlab.Core.Entities;
using Medlab.Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MedlabDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


//identity


builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 8;

}).AddDefaultTokenProviders().AddEntityFrameworkStores<MedlabDbContext>();


// google auth
builder.Services.AddAuthentication()
               .AddGoogle(options =>
               {
                   options.ClientId = "452097516219-f1gkr0obf8bq4jqhgngp58mea3meedah.apps.googleusercontent.com";
                   options.ClientSecret = "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";
                   options.SignInScheme = IdentityConstants.ExternalScheme;
               });

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

using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Medlab.Core.Entities;
using Medlab.Data.DAL;
using Swashbuckle.AspNetCore.Swagger;
using MedlabApi.Services.Interfaces;
using MedlabApi.Services.Implementations;
using Medlab.Data.Repositories;
using Medlab.Core.Repositories;
using AutoMapper;
using MedlabApi.Profiles;
using MedlabApi.Dtos.SettingDtos;
using MedlabApi.Jobs;
using Quartz.Impl;
using Quartz.Spi;
using Quartz;
using static Quartz.Logging.OperationName;

var builder = WebApplication.CreateBuilder(args);


//       CONTENT
// ---------------------
// 1 Fluent Validation, NewtonsoftJson
// 2 DataBase
// 3 identity
// 4 Custom Services
// 5 Swager 
// 6 Jwt Auth
// 7 Cors addcors
// 8 Mapper
// 9 Quartz


//-----------------
// 1 Cors usecors
// 2 Quartz


builder.Services.AddControllers()

//===============
//1 Fluent validation, NewtonsoftJson
//===============
    .AddNewtonsoftJson(opt =>
                          opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
    .AddFluentValidation(x=> x.RegisterValidatorsFromAssemblyContaining<SettingGetDto>());


// ==========================
// 2 DataBase
// ==========================

builder.Services.AddDbContext<MedlabDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});




//==================
// 3 identity
//==================
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequiredLength = 8;

}).AddDefaultTokenProviders().AddEntityFrameworkStores<MedlabDbContext>();




//===================
// 4 Custom Services
//===================

builder.Services.AddSingleton<IJwtService, JwtService>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<ISettingRepository, SettingRepository>();
builder.Services.AddScoped<ISubscriptionRepostiory, SubscriptionRepository>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


//Product
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductTagRepository, ProductTagRepository>();
builder.Services.AddScoped<IBasketItemRepository, BasketItemRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();


//Blogs
builder.Services.AddScoped<IBlogRepostiory, BlogRepository>();
builder.Services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();

//Doctors
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDoctorAppointmentRepository, DoctorAppointmentRepository>();

//Order
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();

//ContactMessage
builder.Services.AddScoped<IContactMessageRepository, ContactMessageRepository>();

//===================
// 5 Swager 
//===================

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
    opt.AddFluentValidationRules();
});


//===================
// 6 Jwt Auth
//===================


builder.Services.AddAuthentication(opt =>
{
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidAudience = builder.Configuration.GetSection("JWT:audience").Value,
        ValidIssuer = builder.Configuration.GetSection("JWT:issuer").Value,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:secret").Value))

    };
});



//======================
// 7 Cors addcors
//======================

builder.Services.AddCors();

//======================
// 8 Mapper
//======================

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AdminMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IWebHostEnvironment>(), provider.GetService<IConfiguration>()));
}).CreateMapper());


//======================
// 9 Quartz
//======================

builder.Services.AddSingleton<IJobFactory, MyJobFactory>();
builder.Services.AddScoped<EmailSendingJob>();
builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
builder.Services.AddSingleton(provider =>
{
    var schedulerFactory = new StdSchedulerFactory();
    var scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();
    scheduler.JobFactory = provider.GetRequiredService<IJobFactory>();

    var dataMap = new JobDataMap();
    dataMap["serviceProvider"] = provider;

    scheduler.Start();
    return scheduler;
});
builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

//======================
// 2 Quartz
//======================
var scheduler = app.Services.GetRequiredService<IScheduler>();


var job = JobBuilder.Create<EmailSendingJob>()
    .WithIdentity("sendEmailJob", "emailGroup")
    .Build();

var trigger = TriggerBuilder.Create()
    .WithIdentity("sendEmailTrigger", "emailGroup")
    .WithSchedule(CronScheduleBuilder
           //.WeeklyOnDayAndHourAndMinute(DayOfWeek.Monday, 9, 0))
           .CronSchedule("0 */1 * ? * *")) // run every 5 minute
    .Build();

scheduler.ScheduleJob(job, trigger);


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();






// Cli Commands
// cd Medlab.Data
// dotnet ef  --startup-project ..\MedlabApi migrations  add 
// dotnet ef  --startup-project ..\MedlabApi database update


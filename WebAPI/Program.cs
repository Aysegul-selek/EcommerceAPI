using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
<<<<<<< HEAD
using Application.Interfaces.Services.Application.Interfaces.Services;
using Application.MappingProfiles;
using Application.Pipelines.Order;
using Application.Services;
using AspNetCoreRateLimit;
=======
using Application.MappingProfiles;
using Application.Services;
>>>>>>> DevB-1
using AutoMapper;
using Infrastructure;
using Infrastructure.DataBase;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
<<<<<<< HEAD
using WebAPI.Extensions;
using WebAPI.Middleware;
=======

>>>>>>> DevB-1

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// cors ayarları
builder.Services.AddCorsPolicy();

builder.Services.AddMemoryCache();

// appsettings.json dosyasından ayarları okur ve servis konteynerine ekler
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimiting"));

// IHttpContextAccessor hizmetini ekler, rate limiting için gereklidir
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Limit bilgilerini bellek içi (in-memory) depolamak için gerekli hizmeti ekler.
builder.Services.AddInMemoryRateLimiting();

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddControllers();

// Cache için
builder.Services.AddMemoryCache();

// Cache için
builder.Services.AddMemoryCache();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();


// Services
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductManager>();
<<<<<<< HEAD
builder.Services.AddScoped<IRoleService, RoleManager>();
builder.Services.AddScoped<IDiscountStrategy, PercentageDiscountStrategy>();
builder.Services.AddScoped<IDiscountStrategy, FixedDiscountStrategy>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
// Pipeline adımları
builder.Services.AddScoped<IOrderPipelineStep, StockCheckStep>();
builder.Services.AddScoped<IOrderPipelineStep, TotalCalculationStep>();
builder.Services.AddScoped<IOrderPipelineStep, DiscountStep>();
=======

>>>>>>> DevB-1

// OrderFactory
builder.Services.AddScoped<OrderFactory>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT ayarları
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseCors();

app.UseCustomMiddlewares(TimeSpan.FromSeconds(1));
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Interfaces.Services.Application.Interfaces.Services;
using Application.MappingProfiles;
using Application.Pipelines.Order;
using Application.Services;
using AspNetCoreRateLimit;
using Application.MappingProfiles;
using Application.Services;
using AutoMapper;
using Infrastructure;
using Infrastructure.DataBase;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

using WebAPI.Extensions;
using WebAPI.Middleware;
using Serilog;
using Infrastructure.HealthChecks;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Serilog yapılandırması
builder.Host.UseSerilog((ctx, lc) => lc
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext() // CorrelationId, Middleware içinden LogContext’e push ediliyor
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] [CorrelationId:{CorrelationId}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] [CorrelationId:{CorrelationId}] {Message:lj}{NewLine}{Exception}")
);

// HealthChecks servisini ekleme
builder.Services.AddHealthChecks()
    .AddCheck<MsSqlHealthCheck>("mssql-check");

builder.Services.AddSingleton(new MsSqlHealthCheck(builder.Configuration.GetConnectionString("DefaultConnection")));

// cors ayarları
builder.Services.AddCorsPolicy();

builder.Services.AddMemoryCache();

// Rate limiting
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Controllers
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

builder.Services.AddScoped<IRoleService, RoleManager>();
builder.Services.AddScoped<IDiscountStrategy, PercentageDiscountStrategy>();
builder.Services.AddScoped<IDiscountStrategy, FixedDiscountStrategy>();
builder.Services.AddScoped<IDiscountService, DiscountService>();
builder.Services.AddScoped<IIdempotencyRequestRepository, IdempotencyRequestRepository>();
builder.Services.AddScoped<IdempotencyService>();
// Pipeline adımları
builder.Services.AddScoped<IOrderPipelineStep, StockCheckStep>();
builder.Services.AddScoped<IOrderPipelineStep, TotalCalculationStep>();
builder.Services.AddScoped<IOrderPipelineStep, DiscountStep>();


// OrderFactory
builder.Services.AddScoped<OrderFactory>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseCors();

app.UseMiddleware<CorrelationIdMiddleware>();


app.UseCustomMiddlewares(TimeSpan.FromSeconds(20));
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

app.MapHealthChecks("/health");// healtyCheck endpointi

app.Run();

using Application.Dtos.ResponseDto;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Services;
using Infrastructure.DataBase;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Application.MappingProfiles;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// HttpContext accessor 
builder.Services.AddHttpContextAccessor();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();



// Services
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IOrderService, OrderManager>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IRoleService, RoleManager>();


// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));


//jwt ayarlar?
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

// Global Exception Middleware
app.UseMiddleware<WebAPI.Middleware.GlobalExceptionMiddleware>();

// yetkisiz olma durumunu yakalama
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 401 || response.StatusCode == 403)
    {
        response.ContentType = "application/json";

        var result = new ApiResponseDto<object>
        {
            Success = false,
            Message = response.StatusCode == 401
                ? "Kullanıcı yetkili değil veya token geçersiz."
                : "Erişim yetkiniz yok.",
            Data = null,
            ErrorCodes = ErrorCodes.Unauthorized
        };

        var json = System.Text.Json.JsonSerializer.Serialize(result);
        await response.WriteAsync(json);
    }
});


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.Run();

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Infrastructure.DataBase
{
    public class AppDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public override Task<int> SaveChangesAsync(System.Threading.CancellationToken cancellationToken = default)
        {
            var currentUserEmail = _httpContextAccessor.HttpContext?.User?.Claims
                .FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreateUser = currentUserEmail;
                    entry.Entity.IsDeleted = false;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    entry.Entity.UpUser = currentUserEmail;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Soft Delete Global Query Filter
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(GetIsDeletedRestriction(entityType.ClrType));
                }
            }

            // Decimal precision ayarları
            modelBuilder.Entity<Order>(builder =>
            {
                builder.Property(o => o.Total).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.Property(oi => oi.UnitPrice).HasColumnType("decimal(18,2)");
            });
        }

        private static LambdaExpression GetIsDeletedRestriction(Type type)
        {
            var param = Expression.Parameter(type, "e");
            var prop = Expression.Property(param, nameof(BaseEntity.IsDeleted));
            var condition = Expression.Equal(prop, Expression.Constant(false));
            return Expression.Lambda(condition, param);
        }
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Order decimal precision
            modelBuilder.Entity<Order>(builder =>
            {
                builder.Property(o => o.Total)
                       .HasColumnType("decimal(18,2)");
            });

            // OrderItem decimal precision
            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.Property(oi => oi.UnitPrice)
                       .HasColumnType("decimal(18,2)");
            });
        }
    }
}

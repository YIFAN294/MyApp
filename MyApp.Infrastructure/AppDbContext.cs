using Microsoft.EntityFrameworkCore;
using MyApp.Models;

namespace MyApp.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置种子数据
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "管理员11",
                    Email = "admin@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}

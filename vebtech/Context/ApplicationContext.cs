using Microsoft.EntityFrameworkCore;
using vebtech.Models;

namespace vebtech.Context;
public class ApplicationContext : DbContext
{
    private readonly IConfiguration _configuration;

    public ApplicationContext(DbContextOptions<ApplicationContext> options,
        IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Admin> Admins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Admin>().HasData(
        new Admin[]
        {
            new Admin {
                Id = 1,
                Email = _configuration["AdminUser:Email"],
                Password = BCrypt.Net.BCrypt.HashPassword(_configuration["AdminUser:Password"])
            },
        });
        modelBuilder.Entity<User>().HasData(
            new User[]
            {
                new User { Id = 1, Name = "User 1", Age = 25, Email = "user1@example.com" },
                new User { Id = 2, Name = "User 2", Age = 30, Email = "user2@example.com" },
                new User { Id = 3, Name = "User 3", Age = 35, Email = "user3@example.com" },
                new User { Id = 4, Name = "User 4", Age = 40, Email = "user4@example.com" },
                new User { Id = 5, Name = "User 5", Age = 45, Email = "user5@example.com" },
                new User { Id = 6, Name = "User 6", Age = 50, Email = "user6@example.com" }
            });
        modelBuilder.Entity<Role>().HasData(
            new Role[]
            {
                new Role { Id = 1, Name = "Role 1", UserId = 1 },
                new Role { Id = 2, Name = "Role 2", UserId = 1 },
                new Role { Id = 3, Name = "Role 3", UserId = 1 },
                new Role { Id = 4, Name = "Role 1", UserId = 2 },
                new Role { Id = 5, Name = "Role 2", UserId = 2 },
                new Role { Id = 6, Name = "Role 1", UserId = 3 },
                new Role { Id = 7, Name = "Role 4", UserId = 3 },
                new Role { Id = 8, Name = "Role 5", UserId = 3 },
                new Role { Id = 9, Name = "Role 6", UserId = 3 },
                new Role { Id = 10, Name = "Role 7", UserId = 4 },
                new Role { Id = 11, Name = "Role 8", UserId = 4 },
                new Role { Id = 12, Name = "Role 3", UserId = 5 },
                new Role { Id = 13, Name = "Role 8", UserId = 5 },
                new Role { Id = 14, Name = "Role 9", UserId = 5 },
                new Role { Id = 15, Name = "Role 10", UserId = 5 },
                new Role { Id = 16, Name = "Role 12", UserId = 6 },
                new Role { Id = 17, Name = "Role 11", UserId = 6 },
                new Role { Id = 18, Name = "Role 18", UserId = 6 },
            });
        modelBuilder.Entity<Role>()
        .HasOne(r => r.User)
        .WithMany(u => u.Roles)
        .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace JoygameInventory.Data.Context
{
    public class InventoryContext : IdentityDbContext<JoyUser, JoyRole, string>
    {
        public InventoryContext(DbContextOptions<InventoryContext> options) : base(options)
        {
        }

        // DbSet tanımlamaları
        public DbSet<Product> Products => Set<Product>();
        public DbSet<InventoryAssigment> InventoryAssigments => Set<InventoryAssigment>();

        public DbSet<JoyUser> JoyUsers => Set<JoyUser>();
        public DbSet<JoyRole> JoyRoles => Set<JoyRole>();

        // Seed Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // InventoryAssigment için Fluent API ayarları
            modelBuilder.Entity<InventoryAssigment>()
                .HasKey(ia => ia.Id);

            modelBuilder.Entity<InventoryAssigment>()
                .HasOne(ia => ia.Product)  // InventoryAssigment bir Product ile ilişkili
                .WithMany(p => p.InventoryAssigments)  // Bir Product birden fazla InventoryAssigment ile ilişkili
                .HasForeignKey(ia => ia.ProductId);

            modelBuilder.Entity<InventoryAssigment>()
                .HasOne(ia => ia.User)  // InventoryAssigment bir User ile ilişkili
                .WithMany(u => u.InventoryAssigments)  // Bir User birden fazla InventoryAssigment ile ilişkili
                .HasForeignKey(ia => ia.UserId);

            // Add Products (Ürünler)
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, ProductName = "Laptop", Description = "High-performance laptop", SerialNumber = "3872-5930-4832", img = "ürün.jpg",ProductBarkod ="JGNB054" },
                new Product { Id = 2, ProductName = "Mouse", Description = "Wireless mouse", SerialNumber = "3840294-9F5A3C2D", img = "ürün.jpg", ProductBarkod = "JGNB060" },
                new Product { Id = 3, ProductName = "Keyboard", Description = "Mechanical keyboard", SerialNumber = "A2B3-5829-20250111", img = "ürün.jpg", ProductBarkod = "JGNB024" }
            );

            // Add Users (Kullanıcılar) - IdentityUser örneği olarak string Id kullanıyoruz.
            modelBuilder.Entity<JoyUser>().HasData(
                new JoyUser { Id = "1", UserName = "john_doe", Email = "john@example.com", FirstName = "John", LastName = "Doe" },
                new JoyUser { Id = "2", UserName = "jane_doe", Email = "jane@example.com", FirstName = "Jane", LastName = "Doe" }
            );

            // Add Roles (Roller)
            modelBuilder.Entity<JoyRole>().HasData(
                new JoyRole { Id = "1", Name = "Admin" },
                new JoyRole { Id = "2", Name = "User" }
            );

            // Assign roles to users
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(iur => new { iur.UserId, iur.RoleId }); // Composite key for IdentityUserRole
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "1", RoleId = "1" }, // Admin rolü atandı
                new IdentityUserRole<string> { UserId = "2", RoleId = "2" }  // User rolü atandı
            );

            // Add InventoryAssigments (Zimmetler)
            modelBuilder.Entity<InventoryAssigment>().HasData(
                new InventoryAssigment { Id = 1, ProductId = 1, UserId = "1", AssignmentDate = DateTime.UtcNow,Status = "active" },
                new InventoryAssigment { Id = 2, ProductId = 2, UserId = "2", AssignmentDate = DateTime.UtcNow, Status = "active" }
            );
        }
    }
}

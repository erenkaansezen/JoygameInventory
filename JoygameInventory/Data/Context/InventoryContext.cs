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
                 new Product { Id = 1, ProductName = "Laptop", Description = "High-performance laptop", SerialNumber = "3872-5930-4832", img = "laptop.jpg", ProductBarkod = "JGNB054" },
                new Product { Id = 2, ProductName = "Mouse", Description = "Wireless mouse", SerialNumber = "3840294-9F5A3C2D", img = "mouse.jpg", ProductBarkod = "JGNB060" },
                new Product { Id = 3, ProductName = "Keyboard", Description = "Mechanical keyboard", SerialNumber = "A2B3-5829-20250111", img = "keyboard.jpg", ProductBarkod = "JGNB024" },
                new Product { Id = 4, ProductName = "Monitor", Description = "27-inch 4K monitor", SerialNumber = "WLG-384029-2024", img = "monitor.jpg", ProductBarkod = "JGNB095" },
                new Product { Id = 5, ProductName = "Headphones", Description = "Noise-cancelling over-ear headphones", SerialNumber = "HDP-230904", img = "headphones.jpg", ProductBarkod = "JGNB101" },
                new Product { Id = 6, ProductName = "USB Drive", Description = "128GB USB 3.0 Flash Drive", SerialNumber = "USB-3847502", img = "usbdrive.jpg", ProductBarkod = "JGNB112" }



            );

            // Add Users (Kullanıcılar) - IdentityUser örneği olarak string Id kullanıyoruz.
            modelBuilder.Entity<JoyUser>().HasData(
                new JoyUser { Id = "1", UserName = "eren_sezen", Email = "eren.sezen@joygame.com", FirstName = "Eren", LastName = "Sezen" },
                new JoyUser { Id = "2", UserName = "osman_benlice", Email = "osman.benlice@joygame.com", FirstName = "Jane", LastName = "Doe" },
                new JoyUser { Id = "3", UserName = "onur.unlu", Email = "onur.unlu@joygame.com", FirstName = "Onur", LastName = "Ünlü" }

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
                new InventoryAssigment { Id = 2, ProductId = 2, UserId = "2", AssignmentDate = DateTime.UtcNow, Status = "active" },
                new InventoryAssigment { Id = 3, ProductId = 5, UserId = "1", AssignmentDate = DateTime.UtcNow, Status = "active" },
                new InventoryAssigment { Id = 4, ProductId = 6, UserId = "1", AssignmentDate = DateTime.UtcNow, Status = "active" }


            );
        }
    }
}

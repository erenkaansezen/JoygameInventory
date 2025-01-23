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
        public DbSet<AssigmentHistory> AssigmentHistorys => Set<AssigmentHistory>();

        public DbSet<JoyStaff> JoyStaffs => Set<JoyStaff>();
        public DbSet<JoyUser> JoyUsers => Set<JoyUser>();
        public DbSet<JoyRole> JoyRoles => Set<JoyRole>();

        // Seed Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventoryAssigment>(entity =>
            {
                // Primary Key
                entity.HasKey(ia => ia.Id);

                entity.HasOne(ia => ia.Product)  
                      .WithMany(p => p.InventoryAssigments) 
                      .HasForeignKey(ia => ia.ProductId)
                      .OnDelete(DeleteBehavior.Cascade); 


                entity.HasOne(ia => ia.User)
                      .WithMany(u => u.InventoryAssigments) 
                      .HasForeignKey(ia => ia.UserId)
                      .OnDelete(DeleteBehavior.SetNull); 

     
                entity.HasOne(ia => ia.PreviusAssigmentUserNavigation)  
                      .WithMany() 
                      .HasForeignKey(ia => ia.PreviusAssigmenId)
                      .OnDelete(DeleteBehavior.SetNull); 
            });
            modelBuilder.Entity<AssigmentHistory>(entity =>
            {
                // Primary Key
                entity.HasKey(ia => ia.Id);

                entity.HasOne(ia => ia.Product)
                      .WithMany(p => p.AssigmentHistorys)
                      .HasForeignKey(ia => ia.ProductId)
                      .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(ia => ia.User)
                      .WithMany(u => u.AssigmentHistorys)
                      .HasForeignKey(ia => ia.UserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Status)
                      .HasDefaultValue("Depoda"); 
            });

            // Add Products (Ürünler)
            modelBuilder.Entity<Product>().HasData(
                    new Product { Id = 1, ProductName = "Laptop", Description = "High-performance laptop", SerialNumber = "3872-5930-4832", ProductBarkod = "JGNB054",Status="Zimmetli" },
                    new Product { Id = 2, ProductName = "Mouse", Description = "Wireless mouse", SerialNumber = "3840294-9F5A3C2D", ProductBarkod = "JGNB060", Status = "Zimmetli" },
                    new Product { Id = 3, ProductName = "Keyboard", Description = "Mechanical keyboard", SerialNumber = "A2B3-5829-20250111", ProductBarkod = "JGNB024", Status = "Zimmetli" },
                    new Product { Id = 4, ProductName = "Monitor", Description = "27-inch 4K monitor", SerialNumber = "WLG-384029-2024", ProductBarkod = "JGNB095", Status = "Zimmetli" },
                    new Product { Id = 5, ProductName = "Headphones", Description = "Noise-cancelling over-ear headphones", SerialNumber = "HDP-230904", ProductBarkod = "JGNB101", Status = "Zimmetli" },
                    new Product { Id = 6, ProductName = "USB Drive", Description = "128GB USB 3.0 Flash Drive", SerialNumber = "USB-3847502", ProductBarkod = "JGNB112", Status = "Zimmetli" },
                    new Product { Id = 7, ProductName = "Smartphone", Description = "Latest model smartphone with 5G", SerialNumber = "SMP-1234A678"    , ProductBarkod = "JGNB130", Status = "Zimmetli" },
                    new Product { Id = 8, ProductName = "Tablet", Description = "10-inch tablet with stylus support", SerialNumber = "TAB-5467D2025"    , ProductBarkod = "JGNB145", Status = "Zimmetli" },
                    new Product { Id = 9, ProductName = "Smartwatch", Description = "Fitness smartwatch with heart-rate monitor", SerialNumber = "SW-9476253", ProductBarkod = "JGNB162" },
                    new Product { Id = 10, ProductName = "Gaming Mouse", Description = "High-DPI gaming mouse", SerialNumber = "GM-845320"  , ProductBarkod = "JGNB170" },
                    new Product { Id = 11, ProductName = "Laptop Sleeve", Description = "Protective laptop sleeve", SerialNumber = "LS-210987"  , ProductBarkod = "JGNB183" },
                    new Product { Id = 12, ProductName = "Camera", Description = "DSLR camera with 24MP sensor", SerialNumber = "CAM-584230", ProductBarkod = "JGNB195" },
                    new Product { Id = 13, ProductName = "Bluetooth Speaker", Description = "Portable Bluetooth speaker with rich sound", SerialNumber = "BTS-789403", ProductBarkod = "JGNB210" },
                    new Product { Id = 14, ProductName = "Power Bank", Description = "10,000mAh power bank", SerialNumber = "PB-543210", ProductBarkod = "JGNB230" },
                    new Product { Id = 15, ProductName = "VR Headset", Description = "Virtual reality headset for immersive experiences", SerialNumber = "VR-902384", ProductBarkod = "JGNB245" },
                    new Product { Id = 16, ProductName = "External Hard Drive", Description = "2TB external hard drive", SerialNumber = "EHDD-098723", ProductBarkod = "JGNB260" },
                    new Product { Id = 17, ProductName = "Gaming Chair", Description = "Ergonomic gaming chair", SerialNumber = "GC-765493", ProductBarkod = "JGNB275" },
                    new Product { Id = 18, ProductName = "Electric Scooter", Description = "Foldable electric scooter", SerialNumber = "ES-129845", ProductBarkod = "JGNB280" },
                    new Product { Id = 19, ProductName = "Drone", Description = "4K camera drone with flight stabilization", SerialNumber = "DRN-589301", ProductBarkod = "JGNB295" },
                    new Product { Id = 20, ProductName = "Projector", Description = "Portable mini projector", SerialNumber = "PRJ-765123", ProductBarkod = "JGNB310", }




            );

            // Add Users (Kullanıcılar) - IdentityUser örneği olarak string Id kullanıyoruz.
            modelBuilder.Entity<JoyUser>().HasData(
                new JoyUser { Id = "1", UserName = "eren_sezen", Email = "eren.sezen@joygame.com", FirstName = "Eren", LastName = "Sezen" },
                new JoyUser { Id = "2", UserName = "osman_benlice", Email = "osman.benlice@joygame.com", FirstName = "Jane", LastName = "Doe" },
                new JoyUser { Id = "3", UserName = "onur.unlu", Email = "onur.unlu@joygame.com", FirstName = "Onur", LastName = "Ünlü" }

            );
            // Add Users (Kullanıcılar) - IdentityUser örneği olarak string Id kullanıyoruz.
            modelBuilder.Entity<JoyStaff>().HasData(
        new JoyStaff { Id = 1, UserName = "eren_sezen", Email = "eren.sezen@joygame.com", Name = "Eren", Surname = "Sezen", PhoneNumber = "555-0101" },
        new JoyStaff { Id = 2, UserName = "osman_benlice", Email = "osman.benlice@joygame.com", Name = "Osman", Surname = "Benlice", PhoneNumber = "555-0102" },
        new JoyStaff { Id = 3, UserName = "onur_unlu", Email = "onur.unlu@joygame.com", Name = "Onur", Surname = "Ünlü", PhoneNumber = "555-0103" },
        new JoyStaff { Id = 4, UserName = "ali_karatas", Email = "ali.karatas@joygame.com", Name = "Ali", Surname = "Karataş", PhoneNumber = "555-0104" },
        new JoyStaff { Id = 5, UserName = "ayse_duran", Email = "ayse.duran@joygame.com", Name = "Ayşe", Surname = "Duran", PhoneNumber = "555-0105" },
        new JoyStaff { Id = 6, UserName = "fatma_ozdemir", Email = "fatma.ozdemir@joygame.com", Name = "Fatma", Surname = "Özdemir", PhoneNumber = "555-0106" },
        new JoyStaff { Id = 7, UserName = "mehmet_bayar", Email = "mehmet.bayar@joygame.com", Name = "Mehmet", Surname = "Bayar", PhoneNumber = "555-0107" },
        new JoyStaff { Id = 8, UserName = "hasan_sahin", Email = "hasan.sahin@joygame.com", Name = "Hasan", Surname = "Şahin", PhoneNumber = "555-0108" },
        new JoyStaff { Id = 9, UserName = "zeynep_kucuk", Email = "zeynep.kucuk@joygame.com", Name = "Zeynep", Surname = "Küçük", PhoneNumber = "555-0109" },
        new JoyStaff { Id = 10, UserName = "yusuf_bozkurt", Email = "yusuf.bozkurt@joygame.com", Name = "Yusuf", Surname = "Bozkurt", PhoneNumber = "555-0110" }




            );

            // Add Roles (Roller)
            modelBuilder.Entity<JoyRole>().HasData(
                new JoyRole { Id = "1", Name = "Madbyte" },
                new JoyRole { Id = "2", Name = "Joygame" }
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
                    new InventoryAssigment { Id = 1, ProductId = 1, UserId = 1, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3},
                    new InventoryAssigment { Id = 2, ProductId = 2, UserId = 2, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 3, ProductId = 5, UserId = 1, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 4, ProductId = 6, UserId = 2, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },

                    new InventoryAssigment { Id = 5, ProductId = 3, UserId = 3, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 6, ProductId = 4, UserId = 4, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },

                    new InventoryAssigment { Id = 7, ProductId = 7, UserId = 5, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 8, ProductId = 8, UserId = 6, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 9, ProductId = 9, UserId = 6, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 }



            );
        }
    }
}

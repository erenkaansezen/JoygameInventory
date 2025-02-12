using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<UserTeam> userTeam => Set<UserTeam>();
        public DbSet<LicenceUser> LicenceUser => Set<LicenceUser>();


        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<AssigmentHistory> AssigmentHistorys => Set<AssigmentHistory>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Licence> Licence => Set<Licence>();

        public DbSet<Maintenance> Maintenance => Set<Maintenance>();
        public DbSet<MaintenanceHistory> MaintenanceHistory => Set<MaintenanceHistory>();

        public DbSet<Category> Categories => Set<Category>();
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
                      .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(ia => ia.PreviusAssigmentUserNavigation)
                      .WithMany()
                      .HasForeignKey(ia => ia.PreviusAssigmenId)
                      .OnDelete(DeleteBehavior.Cascade);
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
                entity.HasKey(s => s.Id);

                entity.Property(p => p.Status)
                      .HasDefaultValue("Depoda");
                entity.HasMany(p => p.Categories)
                      .WithMany(c => c.Products)
                      .UsingEntity<ProductCategory>();

                entity.HasIndex(s => s.ProductBarkod)
                       .IsUnique()
                      .HasDatabaseName("IX_Product_ProductBarkod");
            });
            modelBuilder.Entity<JoyStaff>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_Staff_Email");
            });
            modelBuilder.Entity<Category>(entity =>
            {
                // Primary Key and Default Value
                entity.HasKey(p => p.Id);

                entity.HasIndex(u => u.Url)
                .IsUnique();
            });
            modelBuilder.Entity<ProductCategory>(entity =>
            {
                // Birincil Anahtar
                entity.HasKey(ia => ia.Id);

                // Ürün ile ilişkiyi kuruyoruz
                entity.HasOne(pc => pc.Product)
                      .WithMany(p => p.ProductCategories)
                      .HasForeignKey(pc => pc.ProductId)
                      .OnDelete(DeleteBehavior.Cascade); 

                // Kategori ile ilişkiyi kuruyoruz
                entity.HasOne(pc => pc.Category)
                      .WithMany(c => c.ProductCategories)
                      .HasForeignKey(pc => pc.CategoryId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });
            modelBuilder.Entity<UserTeam>(entity =>
            {
                entity.HasKey(ia => ia.Id);


                entity.HasOne(pc => pc.Staff)
                      .WithMany(p => p.Teams)
                      .HasForeignKey(pc => pc.StaffId)
                      .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(pc => pc.Team)
                      .WithMany(c => c.Teams)
                      .HasForeignKey(pc => pc.TeamId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });
            modelBuilder.Entity<LicenceUser>(entity =>
            {
                entity.HasKey(ia => ia.Id);

                entity.HasOne(pc => pc.staff)
                      .WithMany(p => p.LicenceUser)
                      .HasForeignKey(pc => pc.StaffId)
                      .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(pc => pc.Licence)
                      .WithMany(c => c.LicenceUser)
                      .HasForeignKey(pc => pc.LicenceId)
                      .OnDelete(DeleteBehavior.Cascade); 
            });
            modelBuilder.Entity<JoyUser>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_User_Email");
            });
            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.TeamName)
                      .IsUnique()
                      .HasDatabaseName("IX_Team_TeamName");
            });
            modelBuilder.Entity<Licence>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.HasIndex(s => s.LicenceName)
                      .IsUnique()
                      .HasDatabaseName("IX_Licence_LicenceName");
            });
            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.HasKey(ia => ia.Id);

                entity.HasOne(pc => pc.Product)
                      .WithMany(p => p.Maintenances)
                      .HasForeignKey(pc => pc.ProductBarkod)
                      .HasPrincipalKey(p => p.ProductBarkod);
                entity.HasIndex(s => s.ProductBarkod)
                      .IsUnique();

            });
            modelBuilder.Entity<MaintenanceHistory>(entity =>
            {
                entity.HasKey(ia => ia.Id);

                entity.HasOne(pc => pc.Product)
                      .WithMany(p => p.MaintenanceHistory)
                      .HasForeignKey(pc => pc.ProductBarkod)
                      .HasPrincipalKey(p => p.ProductBarkod);


            });

            modelBuilder.Entity<Maintenance>().HasData(
            new Maintenance { Id = 1, MaintenanceDescription = "Laptop1", CreatedAt = new DateTime(2024, 02, 01), ProductBarkod = "JGNB054" },
            new Maintenance { Id = 2, MaintenanceDescription = "Ekipman1", CreatedAt = new DateTime(2024, 02, 01), ProductBarkod = "JGNB060" }
            );

            modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, ProductName = "Laptop1", Description = "High-performance laptop", SerialNumber = "3872-5930-4832", ProductBarkod = "JGNB054" },
            new Product { Id = 2, ProductName = "Ekipman1", Description = "Wireless mouse", SerialNumber = "3840294-9F5A3C2D", ProductBarkod = "JGNB060" },
            new Product { Id = 3, ProductName = "Ekipman2", Description = "Mechanical keyboard", SerialNumber = "A2B3-5829-20250111", ProductBarkod = "JGNB024" },
            new Product { Id = 4, ProductName = "Ekipman3", Description = "27-inch 4K monitor", SerialNumber = "WLG-384029-2024", ProductBarkod = "JGNB095" },
            new Product { Id = 5, ProductName = "Ekipman4", Description = "Noise-cancelling over-ear headphones", SerialNumber = "HDP-230904", ProductBarkod = "JGNB101" },
            new Product { Id = 16, ProductName = "Desktop1", Description = "2TB external hard drive", SerialNumber = "EHDD-098723", ProductBarkod = "JGNB260" },
            new Product { Id = 18, ProductName = "Desktop2", Description = "Foldable electric scooter", SerialNumber = "ES-129845", ProductBarkod = "JGNB280" },
            new Product { Id = 19, ProductName = "Donanım1", Description = "4K camera drone with flight stabilization", SerialNumber = "DRN-589301", ProductBarkod = "JGNB295" },
            new Product { Id = 20, ProductName = "Donanım2", Description = "Portable mini projector", SerialNumber = "PRJ-765123", ProductBarkod = "JGNB310" }
            );

            modelBuilder.Entity<Category>().HasData(
                new List<Category>()
                {
                            new() {Id=1,Name="Masaüstü",Url="Masaüstü"},
                            new() {Id=2,Name="Notebook",Url="Notebook"},
                            new() {Id=3,Name="Ekipman",Url="Ekipman"},
                            new() {Id=4,Name="Donanım",Url="Donanım"},
                }

            );
            modelBuilder.Entity<Licence>().HasData(
                new List<Licence>()
                {
                            new() {Id=1,LicenceName="Adobe Creative Cloud",LicenceActiveDate=new DateTime(2024, 02, 01),LicenceEndDate=new DateTime(2025, 02, 01)},
                            new() {Id=2,LicenceName="Adobe Photoshop",LicenceActiveDate=new DateTime(2024, 02, 01),LicenceEndDate=new DateTime(2025, 02, 01)},
                            new() {Id=3,LicenceName="Adobe Substance",LicenceActiveDate=new DateTime(2024, 02, 01),LicenceEndDate=new DateTime(2025, 02, 01)},
                            new() {Id=4,LicenceName="Autodesk 3dmax",LicenceActiveDate=new DateTime(2024, 02, 01),LicenceEndDate=new DateTime(2025, 02, 01)},
                }

            );
            modelBuilder.Entity<Team>().HasData(
                new List<Team>()
                {
                            new() {Id=1,TeamName="Madbyte"},
                            new() {Id=2,TeamName="JoyGame"},
                            new() {Id=3,TeamName="DesertWarrior"},
                            new() {Id=4,TeamName="Growth"},
                            new() {Id=5,TeamName="AI"},
                }

            );
            modelBuilder.Entity<LicenceUser>().HasData(
                    new List<LicenceUser>() 
                    {
                                    new LicenceUser() { Id = 1, StaffId = 1, LicenceId = 1 },
                                    new LicenceUser() { Id = 2, StaffId = 2, LicenceId = 1 },
                                    new LicenceUser() { Id = 3, StaffId = 3, LicenceId = 2 },
                                    new LicenceUser() { Id = 4, StaffId = 4, LicenceId = 2 },
                                    new LicenceUser() { Id = 5, StaffId = 5, LicenceId = 3 },
                                    new LicenceUser() { Id = 6, StaffId = 6, LicenceId = 3 },
                                    new LicenceUser() { Id = 7, StaffId = 7, LicenceId = 4 },
                                    new LicenceUser() { Id = 8, StaffId = 8, LicenceId = 4 },
                                    new LicenceUser() { Id = 9, StaffId = 9, LicenceId = 4 },
                    }
                );
            modelBuilder.Entity<UserTeam>().HasData(
                    new List<UserTeam>()
                    {
                                    new UserTeam() { Id = 1, StaffId = 1, TeamId = 1 },
                                    new UserTeam() { Id = 2, StaffId = 2, TeamId = 1 },
                                    new UserTeam() { Id = 3, StaffId = 3, TeamId = 2 },
                                    new UserTeam() { Id = 4, StaffId = 4, TeamId = 2 },
                                    new UserTeam() { Id = 5, StaffId = 5, TeamId = 3 },
                                    new UserTeam() { Id = 6, StaffId = 6, TeamId = 3 },
                                    new UserTeam() { Id = 7, StaffId = 7, TeamId = 4 },
                                    new UserTeam() { Id = 8, StaffId = 8, TeamId = 4 },
                                    new UserTeam() { Id = 9, StaffId = 9, TeamId = 5 },
                                    new UserTeam() { Id = 10, StaffId = 10, TeamId = 5 },
                    }
                );
            modelBuilder.Entity<ProductCategory>().HasData(
                new List<ProductCategory>()
                {
                    new ProductCategory() { Id = 1, ProductId = 16, CategoryId = 1 },
                    new ProductCategory() { Id = 2, ProductId = 18, CategoryId = 1 },

                    new ProductCategory() { Id = 3, ProductId = 1, CategoryId = 2 },

                    new ProductCategory() { Id = 4, ProductId = 2, CategoryId = 3 },
                    new ProductCategory() { Id = 5, ProductId = 3, CategoryId = 3 },
                    new ProductCategory() { Id = 6, ProductId = 4, CategoryId = 3 },
                    new ProductCategory() { Id = 7, ProductId = 5, CategoryId = 3 },

                    new ProductCategory() { Id = 8, ProductId = 19, CategoryId = 4 },
                    new ProductCategory() { Id = 9, ProductId = 20, CategoryId = 4 }



                    // ıd kesişimleri uniq olmalıdır
                }
            );
            // Add Users (Kullanıcılar) - IdentityUser örneği olarak string Id kullanıyoruz.
            modelBuilder.Entity<JoyUser>().HasData(
                new JoyUser { Id = "1", UserName = "eren_sezen", Email = "eren.sezen@joygame.com", FirstName = "Eren", LastName = "Sezen" },
                new JoyUser { Id = "2", UserName = "osman_benlice", Email = "osman.benlice@joygame.com", FirstName = "Jane", LastName = "Doe" },
                new JoyUser { Id = "3", UserName = "onur.unlu", Email = "onur.unlu@joygame.com", FirstName = "Onur", LastName = "Ünlü" }

            );
            // Add Users (Kullanıcılar) - IdentityUser örneği olarak string Id kullanıyoruz.
            modelBuilder.Entity<JoyStaff>().HasData(
            new JoyStaff { Id = 1, Email = "eren.sezen@joygame.com", Name = "Eren", Surname = "Sezen", PhoneNumber = "555-0101" },
            new JoyStaff { Id = 2, Email = "osman.benlice@joygame.com", Name = "Osman", Surname = "Benlice", PhoneNumber = "555-0102" },
            new JoyStaff { Id = 3, Email = "onur.unlu@joygame.com", Name = "Onur", Surname = "Ünlü", PhoneNumber = "555-0103" },
            new JoyStaff { Id = 4, Email = "ali.karatas@joygame.com", Name = "Ali", Surname = "Karataş", PhoneNumber = "555-0104" },
            new JoyStaff { Id = 5, Email = "ayse.duran@joygame.com", Name = "Ayşe", Surname = "Duran", PhoneNumber = "555-0105" },
            new JoyStaff { Id = 6, Email = "fatma.ozdemir@joygame.com", Name = "Fatma", Surname = "Özdemir", PhoneNumber = "555-0106" },
            new JoyStaff { Id = 7, Email = "mehmet.bayar@joygame.com", Name = "Mehmet", Surname = "Bayar", PhoneNumber = "555-0107" },
            new JoyStaff { Id = 8, Email = "hasan.sahin@joygame.com", Name = "Hasan", Surname = "Şahin", PhoneNumber = "555-0108" },
            new JoyStaff { Id = 9, Email = "zeynep.kucuk@joygame.com", Name = "Zeynep", Surname = "Küçük", PhoneNumber = "555-0109" },
            new JoyStaff { Id = 10, Email = "yusuf.bozkurt@joygame.com", Name = "Yusuf", Surname = "Bozkurt", PhoneNumber = "555-0110" },
            new JoyStaff { Id = 11, Email = "cihad.yilmazer@madbytegames.com", Name = "Cihad", Surname = "Yılmazer", PhoneNumber = "555-0110" }

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
                    new InventoryAssigment { Id = 1, ProductId = 1, UserId = 1, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 2, ProductId = 2, UserId = 2, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 3, ProductId = 3, UserId = 1, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 4, ProductId = 4, UserId = 2, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },

                    new InventoryAssigment { Id = 5, ProductId = 5, UserId = 3, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 6, ProductId = 16, UserId = 4, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },

                    new InventoryAssigment { Id = 7, ProductId = 18, UserId = 5, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 8, ProductId = 19, UserId = 6, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 },
                    new InventoryAssigment { Id = 9, ProductId = 20, UserId = 6, AssignmentDate = DateTime.UtcNow, PreviusAssigmenId = 3 }



            );
        }
    }
}

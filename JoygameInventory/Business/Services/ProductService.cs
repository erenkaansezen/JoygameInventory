using JoygameInventory.Business.Interface;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace JoygameInventory.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;
        private readonly IMaintenanceService _maintenanceService;
        private readonly EmailService _emailService;
        private readonly IAssigmentService _assignmentService;
        private readonly IJoyStaffService _staffManager;
        private readonly ICategoryService _categoryService;
        public ProductService(InventoryContext context,IMaintenanceService maintenanceService,IAssigmentService assigmentService,IJoyStaffService joyStaffService,ICategoryService categoryService,EmailService emailService)
        {
            _context = context;
            _assignmentService = assigmentService;
            _maintenanceService = maintenanceService;
            _staffManager = joyStaffService;
            _categoryService = categoryService;
            _emailService = emailService;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = _context.Products
                .Include(p => p.InventoryAssigments)
                    .ThenInclude(ia => ia.User)
                .ToList();

            foreach (var product in products)
            {
                var maintenance = await _context.Maintenance
                    .FirstOrDefaultAsync(m => m.ProductBarkod == product.ProductBarkod);

                if (maintenance != null)
                {
                    product.Status = "Servis";
                }
                else if (product.InventoryAssigments == null || !product.InventoryAssigments.Any())
                {
                    product.Status = "Depoda";
                }
                else
                {
                    product.Status = "Zimmetli";
                }
            }

            return products;
        }

        public async Task<Product> GetIdProductAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(s => s.Id == id);

        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryUrl)
        {
            // Belirtilen kategoriye ait ürünleri getir
            var products = await _context.Products
                                 .Where(p => p.Categories.Any(c => c.Url == categoryUrl))
                                                 .Include(p => p.InventoryAssigments)
                                 .ToListAsync();
            foreach (var product in products)
            {
                if (product.InventoryAssigments == null || !product.InventoryAssigments.Any())
                {
                    product.Status = "Depoda";
                }
                else
                {
                    product.Status = "Zimmetli";
                }
            }
            return products;

        }

        public async Task<ProductEditViewModel> GetCreateViewAsync()
        {
            var categories = await GetAllCategoriesAsync();
            return new ProductEditViewModel
            {
                Categories = categories
            };
        }

        public async Task<bool> CreateProduct(ProductEditViewModel model)
        {
            // Ürünü oluştur
            var product = new Product
            {
                ProductName = model.ProductName,
                ProductBarkod = model.ProductBarkod,
                Description = model.Description,
                SerialNumber = model.SerialNumber,
                ProductBrand = model.ProductBrand,
                ProductModel = model.ProductModel
            };

            // Ürünü kaydet
            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync();

            // Eğer ürün başarılı bir şekilde kaydedildiyse
            if (result > 0)
            {
                model.Id = product.Id;
                var productCategory = new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = model.SelectedCategoryId
                };

                await AddProductCategory(productCategory);

                // Mail gönderme işlemi
                var toEmailAddress = "itsupport@joygame.com"; // ya da dinamik olarak seçilen kullanıcı e-posta adresi
                var subject = "Envantere Yeni Ürün Eklendi!";
                var body = $"<html><head></head><body>" +
                           $"<p>Merhaba,</p>" +
                           $"<p>Aşağıda belirtilen ürün Joygame Zimmetine eklenmiştir.</p>" +
                           $"<p><strong>Ürün :</strong> {model.ProductBrand} {model.ProductModel}</p>" +
                           $"<p><strong>Ürünün Seri Numarası :</strong> {model.SerialNumber}</p>" +
                           $"<p><strong>Envanter Barkodu :</strong> {model.ProductBarkod}</p>" +
                           $"<p>Teşekkürler</p>" +
                           $"</body></html>";

                var attachmentPaths = new List<string>(); // Ek dosya varsa burada tanımlayabilirsiniz
                await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);

                return true; // Başarılı şekilde işlem tamamlandı
            }

            return false; // Eğer ürün kaydedilemediyse false döner
        }

        



        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetProductAsync(string searchTerm, string category)
        {
            if (!string.IsNullOrEmpty(category))
            {
                return await GetProductsByCategoryAsync(category);
            }
            if (string.IsNullOrEmpty(searchTerm))
            {
                return await GetAllProductsAsync();
            }
            else
            {
                return await SearchProduct(searchTerm);
            }

        }
        public async Task<IEnumerable<ProductCategory>> GetProductCategoryAsync(int categoryId)
        {
            var inventoryAssignments = await _context.ProductCategories
                .Include(ia => ia.Category)
                .Include(ia => ia.Product)  
                .Where(ia => ia.ProductId == categoryId)  
                .ToListAsync();

            return inventoryAssignments;
        }
        public async Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }
        public async Task<ProductEditViewModel> GetProductDetailsAsync(int id)
        {
            var staff = await GetIdProductAsync(id);
            if (staff == null) return null;

            var maintenance = await _maintenanceService.GetProductServiceAsync(staff.ProductBarkod);
            var maintenanceHistory = await _maintenanceService.GetProductServiceHistoryAsync(staff.ProductBarkod);
            var inventoryAssignments = await _assignmentService.GetProductAssignmentsAsync(staff.Id);
            var previousAssignments = await _assignmentService.GetPreviousAssignmentsAsync(staff.Id);
            var assignmentHistorys = await _assignmentService.GetAssignmentHistoryAsync(id);
            var productCategory = await GetProductCategoryAsync(staff.Id);
            var category = await GetAllCategoriesAsync();
            var joyStaff = await _staffManager.GetAllStaffsAsync();

            return new ProductEditViewModel
            {
                Id = staff.Id,
                ProductName = staff.ProductName,
                ProductBarkod = staff.ProductBarkod,
                Description = staff.Description,
                SerialNumber = staff.SerialNumber,
                ProductAddDate = staff.ProductAddDate,
                ProductBrand = staff.ProductBrand,
                ProductModel = staff.ProductModel,
                Ram = staff.Ram,
                Processor = staff.Processor,
                GraphicsCard = staff.GraphicsCard,
                Storage = staff.Storage,
                Status = staff.Status,
                InventoryAssigments = inventoryAssignments,
                JoyStaffs = joyStaff,
                AssigmentHistorys = assignmentHistorys,
                Categories = category,
                MaintenanceHistorys = maintenanceHistory,
                Maintenance = maintenance,
                ProductCategory = productCategory,
            };
        }
        public async Task AddProductCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
             await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(ProductEditViewModel model)
        {
            var product = await _context.Products.FindAsync(model.Id);
            if (product == null) throw new Exception("Ürün bulunamadı");

            product.ProductName = model.ProductName;
            product.SerialNumber = model.SerialNumber;
            product.ProductBrand = model.ProductBrand;
            product.ProductModel = model.ProductModel;
            product.Description = model.Description;
            product.ProductAddDate = DateTime.Now;
            product.Storage = model.Storage;
            product.Ram = model.Ram;
            product.Processor = model.Processor;
            product.GraphicsCard = model.GraphicsCard;
            product.Categories = model.Categories;

            var currentCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.ProductId == model.Id);
            if (currentCategory == null || currentCategory.CategoryId != model.SelectedCategoryId)
            {
                await UpdateProductCategoryAsync(model.Id, model.SelectedCategoryId);
            }


            // Kullanıcı ataması işlemi
            await HandleProductAssignmentAsync(model);

            
            _context.Update(product);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // İçerideki hatayı loglayabilirsiniz.
                Console.WriteLine(ex.InnerException?.Message); // Veya log kaydı
                throw;
            }

        }
        public async Task HandleProductAssignmentAsync(ProductEditViewModel model)
        {
            var product = await _context.Products.FindAsync(model.Id);
            var currentAssignments = await _assignmentService.GetProductAssignmentsAsync(product.Id);

            if (currentAssignments != null && currentAssignments.Any())
            {
                var currentAssignment = currentAssignments.FirstOrDefault();
                if (currentAssignment != null)
                {
                    if (currentAssignment.UserId != model.SelectedUserId && model.SelectedUserId != null)
                    {
                        await _assignmentService.AddAssignmentHistoryAsync(new AssigmentHistory
                        {
                            ProductId = currentAssignment.ProductId,
                            UserId = currentAssignment.UserId,
                            AssignmentDate = DateTime.Now
                        });

                        currentAssignment.PreviusAssigmenId = currentAssignment.UserId;
                        currentAssignment.UserId = model.SelectedUserId.Value;
                        currentAssignment.AssignmentDate = DateTime.Now;

                        var newUser = await _staffManager.GetStaffByIdAsync(model.SelectedUserId.Value);
                        if (newUser != null)
                        {
                            var toEmailAddress = newUser.Email;
                            var subject = "Yeni Ürün Ataması";
                            var body = $"<html><head></head><body><p>Merhaba <strong>{newUser.Name}</strong>,</p>" +
                                       $"<p>Aşağıda belirtilen ürün zimmetinize eklenmiştir.</p>" +
                                       $"<p><strong>Ürün:</strong> {model.ProductBrand} {model.ProductModel}</p>" +
                                       $"<p><strong>Envanter Barkodu:</strong> {model.ProductBarkod}</p>" +
                                       $"<p>Teşekkürler,</p><p>İyi Çalışmalar</p></body></html>";
                            var attachmentPaths = new List<string>();
                            await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
                        }
                    }
                }
            }
            else if (model.SelectedUserId.HasValue && model.SelectedUserId.Value != 0)
            {
                var newAssignment = new InventoryAssigment
                {
                    ProductId = product.Id,
                    UserId = model.SelectedUserId.Value,
                    AssignmentDate = DateTime.Now
                };

                await _assignmentService.AddAssignmentAsync(newAssignment);

                var newUser = await _staffManager.GetStaffByIdAsync(model.SelectedUserId.Value);
                if (newUser != null)
                {
                    var toEmailAddress = newUser.Email;
                    var subject = "Yeni Ürün Ataması";
                    var body = $"<html><head></head><body><p>Merhaba <strong>{newUser.Name}</strong>,</p>" +
                               $"<p>Aşağıda belirtilen ürün zimmetinize eklenmiştir.</p>" +
                               $"<p><strong>Ürün:</strong> {model.ProductBrand} {model.ProductModel}</p>" +
                               $"<p><strong>Envanter Barkodu:</strong> {model.ProductBarkod}</p>" +
                               $"<p>Teşekkürler,</p><p>İyi Çalışmalar</p></body></html>";
                    var attachmentPaths = new List<string>();
                    await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
                }
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await GetIdProductAsync(id);  
            var deleteProduct = await _context.Products.FindAsync(id);
            if (deleteProduct != null)
            {
                _context.Products.Remove(deleteProduct);
                var result = await _context.SaveChangesAsync();
                // Mail gönderme işlemi
                var toEmailAddress = "itsupport@joygame.com";
                var subject = "Envanterden Ürün Çıkarılması Hakkında";
                var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                           $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                           $"<p style='text-align: center;'>" +
                           $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                           $"</p>" +
                           $"<p>Merhaba</p>" +
                           $"<p>Aşağıda belirtilen ürün Joygame Zimmetinden çıkarılmıştır.</p>" +
                           $"<p><strong>Ürün :</strong> {product.ProductBrand} {product.ProductModel}</p>" +
                           $"<p><strong>Ürünün Seri Numarası :</strong> {product.SerialNumber}</p>" +
                           $"<p><strong>Envanter Barkodu :</strong> {product.ProductBarkod}</p>" +
                           $"<p>Teşekkürler</p>" +
                           $"<p>İyi Çalışmalar</p>" +
                           $"</div></body></html>";

                var attachmentPaths = new List<string>(); // Ek dosya varsa burada tanımlayabilirsiniz
                await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<IEnumerable<Product>> SearchProduct(string searchTerm)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(product => EF.Functions.Like(product.ProductName, "%" + searchTerm + "%") ||
                                              EF.Functions.Like(product.ProductBarkod, "%" + searchTerm + "%"));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> ProductBarkodUnique(string barkod)
        {
            return !await _context.Products.AnyAsync(s => s.ProductBarkod == barkod);
        }

        public Product GetCategoryById(int id)
        {
            // Veritabanından ürün bilgilerini ve ilişkili ProductCategory verilerini alıyoruz
            var product = _context.Products
                                  .Include(p => p.ProductCategories)  // ProductCategories ilişkisini dahil ediyoruz
                                  .ThenInclude(pc => pc.Category)    // Kategori bilgilerini de dahil ediyoruz
                                  .FirstOrDefault(p => p.Id == id);  // Verilen id ile ürünü buluyoruz

            return product;
        }
        public async Task UpdateProductCategoryAsync(int productId, int selectedCategoryId)
        {
            var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(pc => pc.ProductId == productId);
            if (productCategory.CategoryId != selectedCategoryId)
            {
                productCategory.CategoryId = selectedCategoryId;
                _context.ProductCategories.Update(productCategory);

            }
            await _context.SaveChangesAsync();

        }

        public async Task<int?> GetCurrentCategoryIdAsync(int productId)
        {
            var currentCategoryId = await _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.CategoryId)
                .FirstOrDefaultAsync();

            return currentCategoryId;
        }




    }
}

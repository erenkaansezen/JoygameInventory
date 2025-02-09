using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.Model;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class ProductManagementController : Controller
    {
        public ProductService _productservice;
        public JoyStaffService _staffmanager;
        public AssigmentService _assigmentservice;
        private readonly EmailService _emailService;
        public MaintenanceService _maintenanceservice;

        public ProductManagementController(ProductService productservice, JoyStaffService staffmanager,AssigmentService assigmentService, EmailService emailService, MaintenanceService maintenanceservice)
        {
            _productservice = productservice;
            _staffmanager = staffmanager;
            _assigmentservice = assigmentService;
            _emailService = emailService;
            _maintenanceservice = maintenanceservice;
        }


        [HttpGet]
        public async Task<IActionResult> ProductList(string category, string searchTerm)
        {            // İlk olarak tüm ürünleri alıyoruz
            var joyproducts = await _productservice.GetAllProductsAsync();

            // Eğer arama terimi varsa, arama sonuçlarına göre filtreleme yapıyoruz
            if (!string.IsNullOrEmpty(searchTerm))
            {
                joyproducts = await _productservice.SearchProduct(searchTerm); // Kategoriye göre filtreleme
            }

            // Eğer kategori seçilmişse, kategoriye göre filtreleme yapıyoruz
            if (!string.IsNullOrEmpty(category))
            {
                joyproducts = await _productservice.GetProductsByCategoryAsync(category); // Kategoriye göre filtreleme
            }


            return View(joyproducts);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var staff = await _productservice.GetIdProductAsync(id);
            if (staff != null)
            {
                var inventoryAssignments = await _assigmentservice.GetProductAssignmentsAsync(staff.Id);
                var previousAssignments = await _assigmentservice.GetPreviousAssignmentsAsync(staff.Id);
                var assignmentHistorys = await _assigmentservice.GetAssignmentHistoryAsync(id);
                var category = await _assigmentservice.GetAllStaffsAsync();

                var joystaff = await _staffmanager.GetAllStaffsAsync();
                var model = new ProductEditViewModel
                {
                    Id = staff.Id,
                    ProductName = staff.ProductName,
                    ProductBarkod = staff.ProductBarkod,
                    Description = staff.Description,
                    SerialNumber = staff.SerialNumber,
                    ProductAddDate = staff.ProductAddDate,
                    Status = staff.Status,
                    InventoryAssigments = inventoryAssignments,
                    JoyStaffs = joystaff,
                    AssigmentHistorys = assignmentHistorys,
                    Categories = category,



                };

                return View(model);



            }
            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            var productcategory = await _productservice.GetAllProductCategoriesAsync();
            var category = await _assigmentservice.GetAllStaffsAsync();
            var model = new ProductEditViewModel
            {
                Categories = category
            };
            return View(model);

        }


        [HttpPost]
        public async Task<IActionResult> ProductDetails(ProductEditViewModel model)
        {
            var product = await _productservice.GetIdProductAsync(model.Id);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Ürün bilgilerini güncelleme
            product.ProductName = model.ProductName;
            product.SerialNumber = model.SerialNumber;
            product.ProductBarkod = model.ProductBarkod;
            product.Description = model.Description;
            product.ProductAddDate = DateTime.Now;

            // Mevcut atama kaydını alıyoruz
            var currentAssignments = await _assigmentservice.GetProductAssignmentsAsync(product.Id);

            if (currentAssignments != null && currentAssignments.Any())
            {
                var currentAssignment = currentAssignments.FirstOrDefault();

                if (currentAssignment != null)
                {
                    // Kullanıcı değişmişse
                    if (currentAssignment.UserId != model.SelectedUserId && model.SelectedUserId != null)
                    {
                        // Önceki kullanıcıyı AssignmentHistory tablosuna kaydediyoruz
                        var assignmentHistory = new AssigmentHistory
                        {
                            ProductId = currentAssignment.ProductId,
                            UserId = currentAssignment.UserId,  // Eski kullanıcı ID'si
                            AssignmentDate = DateTime.Now
                        };
                        await _assigmentservice.AddAssignmentHistoryAsync(assignmentHistory);

                        // Önceki kullanıcıyı PreviusAssigmenId'ye kaydediyoruz
                        currentAssignment.PreviusAssigmenId = currentAssignment.UserId;

                        // Yeni kullanıcıyı atıyoruz
                        if (model.SelectedUserId != null)
                        {
                            currentAssignment.UserId = model.SelectedUserId.Value;
                        }

                        currentAssignment.AssignmentDate = DateTime.Now;

                        //Yeni kullanıcıya e-posta gönderimi
                        var newUser = await _staffmanager.GetStaffByIdAsync(model.SelectedUserId.Value);
                        if (newUser != null)
                        {
                            var toEmailAddress = newUser.Email;
                            var subject = "Yeni Ürün Ataması";
                            var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'><div style='margin: 20px; background-color: white; padding: 20px;'><p>Merhaba <strong>{newUser.Name}</strong>,</p><p>Size yeni bir ürün ataması yapılmıştır.</p><p><strong>Ürün Adı:</strong> {product.ProductName}</p><p>Teşekkürler.</p></div></body></html>";
                            var attachmentPaths = new List<string>();  // Boş bir liste oluşturuluyor
                            await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
                        }
                        await _assigmentservice.UpdateAssigmentAsync(currentAssignment);
                    }
                    else
                    {
                        // Kullanıcı değişmemişse sadece ürünü güncelliyoruz
                        await _productservice.UpdateProductAsync(product);
                    }

                    TempData["SuccessMessage"] = "Ürün Başarıyla Güncellendi";
                    return RedirectToAction("ProductDetails", new { id = model.Id });
                }
            }
            else if (model.SelectedUserId.HasValue && model.SelectedUserId.Value != 0) // Atama kaydı yoksa ve kullanıcı seçildiyse
            {
                // Yeni bir atama kaydı oluşturuyoruz
                var newAssignment = new InventoryAssigment
                {
                    ProductId = product.Id,
                    UserId = model.SelectedUserId.Value,  // Seçilen kullanıcı
                    AssignmentDate = DateTime.Now,
                    PreviusAssigmenId = null,  // null yapılıyor
                };

                // Yeni atamayı kaydediyoruz
                await _assigmentservice.AddAssignmentAsync(newAssignment);

                // Yeni kullanıcıya e-posta gönderiyoruz
                var newUser = await _staffmanager.GetStaffByIdAsync(model.SelectedUserId.Value);
                if (newUser != null)
                {
                    var toEmailAddress = newUser.Email;
                    var subject = "Yeni Ürün Ataması";
                    var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'><div style='margin: 20px; background-color: white; padding: 20px;'><p>Merhaba <strong>{newUser.Name}</strong>,</p><p>Size yeni bir ürün ataması yapılmıştır.</p><p><strong>Ürün Adı:</strong> {product.ProductName}</p><p>Teşekkürler.</p></div></body></html>";
                    var attachmentPaths = new List<string>();  // Boş bir liste oluşturuluyor
                    await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
                }

                TempData["SuccessMessage"] = "Ürün Başarıyla Güncellendi";
                return RedirectToAction("ProductDetails", new { id = model.Id });
            }

            // Ürün bilgilerini güncelliyoruz (yeni atama olmadıysa)
            await _productservice.UpdateProductAsync(product);
            TempData["SuccessMessage"] = "Ürün Başarıyla Güncellendi";

            return RedirectToAction("ProductDetails", new { id = model.Id });
        }



        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductEditViewModel model)
        {
            // Barkodun benzersiz olup olmadığını kontrol ediyoruz
            if (!await _productservice.ProductBarkodUnique(model.ProductBarkod))
            {
                TempData["ErrorMessage"] = "Girdiğiniz barkod numarası başka ürüne ait";
                return View("ProductCreate", model);
            }

            // Kategori seçilmediği durum için hata mesajı ekliyoruz
            if (model.SelectedCategoryId <= 0)
            {
                TempData["ErrorMessage"] = "Lütfen Kategori Seçiniz";
                return RedirectToAction("ProductCreate", model);
            }
            // Ürün nesnesini oluşturuyoruz
            var product = new Product
            {
                ProductName = model.ProductName,
                ProductBarkod = model.ProductBarkod,
                Description = model.Description,
                SerialNumber = model.SerialNumber,
                Categories = model.Categories
            };



            // Ürünü kaydetmeye başlıyoruz
            var result = await _productservice.CreateProduct(product);

            if (result) // Eğer ürün başarılı bir şekilde kaydedildiyse
            {
                // Ürün kaydedildikten sonra ProductCategory tablosuna ilişkilendirme ekliyoruz
                var productCategory = new ProductCategory
                {
                    ProductId = product.Id, // Ürün ID'si
                    CategoryId = model.SelectedCategoryId // Seçilen kategori ID'si
                };

                // ProductCategory kaydını ekliyoruz
                await _productservice.AddProductCategory(productCategory);

                TempData["SuccessMessage"] = "Ürün başarıyla oluşturuldu ve kategoriyle ilişkilendirildi!";
                return RedirectToAction("ProductDetails", new { id = product.Id });
            }
            else
            {
                TempData["ErrorMessage"] = "Ürün Oluşturulamadı, lütfen girdiğiniz bilgileri kontrol ediniz";
                return RedirectToAction("ProductCreate", model);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> ProductDelete(int id)
        {
            await _productservice.DeleteProductAsync(id);
            return RedirectToAction("ProductList", "ProductManagement");
        }


    }
}

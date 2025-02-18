using JoygameInventory.Business.Interface;
using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace JoygameInventory.Web.Controllers
{
    [Authorize]

    public class ProductManagementController : Controller
    {
        private readonly IProductService _productService;
        private readonly IJoyStaffService _staffManager;
        private readonly IAssigmentService _assigmentService;
        private readonly EmailService _emailService;
        private readonly IMaintenanceService _maintenanceService;

        public ProductManagementController(IProductService productService, IJoyStaffService staffManager, IAssigmentService assigmentService, EmailService emailService, IMaintenanceService maintenanceService)
        {
            _productService = productService;
            _staffManager = staffManager;
            _assigmentService = assigmentService;
            _emailService = emailService;
            _maintenanceService = maintenanceService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList(string category, string searchTerm)
        {
            var joyProducts = await _productService.GetAllProductsAsync();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                joyProducts = await _productService.SearchProduct(searchTerm);
            }

            if (!string.IsNullOrEmpty(category))
            {
                joyProducts = await _productService.GetProductsByCategoryAsync(category);
            }

            return View(joyProducts);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var staff = await _productService.GetIdProductAsync(id);
            if (staff != null)
            {
                var maintenance = await _maintenanceService.GetProductServiceAsync(staff.ProductBarkod);
                var maintenanceHistory = await _maintenanceService.GetProductServiceHistoryAsync(staff.ProductBarkod);
                var inventoryAssignments = await _assigmentService.GetProductAssignmentsAsync(staff.Id);
                var productCategory = await _productService.GetProductCategoryAsync(staff.Id);
                var previousAssignments = await _assigmentService.GetPreviousAssignmentsAsync(staff.Id);
                var assignmentHistorys = await _assigmentService.GetAssignmentHistoryAsync(id);
                var category = await _productService.GetAllCategoriesAsync();

                var joyStaff = await _staffManager.GetAllStaffsAsync();
                var model = new ProductEditViewModel
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

                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ProductCreate()
        {
            var productCategory = await _productService.GetAllProductCategoriesAsync();
            var category = await _productService.GetAllCategoriesAsync();
            var model = new ProductEditViewModel
            {
                Categories = category
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDetails(ProductEditViewModel model)
        {
            var currentCategoryId = await _productService.GetCurrentCategoryIdAsync(model.Id);
            var product = await _productService.GetIdProductAsync(model.Id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Ürünü kaydederken bir hata oluştu!";
                return RedirectToAction("ProductDetails", model);
            }
            if (!await _productService.ProductBarkodUnique(model.ProductBarkod))
            {
                TempData["ErrorMessage"] = $"{model.ProductBarkod} bu barkod başka ürün için kullanımda";
                return RedirectToAction("ProductDetails", model);
            }

            product.ProductName = model.ProductName;
            product.SerialNumber = model.SerialNumber;
            product.ProductBarkod = model.ProductBarkod;
            product.ProductBrand = model.ProductBrand;
            product.ProductModel = model.ProductModel;
            product.Description = model.Description;
            product.ProductAddDate = DateTime.Now;
            product.Storage = model.Storage;
            product.Ram = model.Ram;
            product.Processor = model.Processor;
            product.GraphicsCard = model.GraphicsCard;
            product.Categories = model.Categories;

            await _productService.UpdateProductCategoryAsync(model.Id, model.SelectedCategoryId);

            var currentAssignments = await _assigmentService.GetProductAssignmentsAsync(product.Id);

            if (currentAssignments != null && currentAssignments.Any())
            {
                var currentAssignment = currentAssignments.FirstOrDefault();

                if (currentAssignment != null)
                {
                    if (currentAssignment.UserId != model.SelectedUserId && model.SelectedUserId != null)
                    {
                        var assignmentHistory = new AssigmentHistory
                        {
                            ProductId = currentAssignment.ProductId,
                            UserId = currentAssignment.UserId,
                            AssignmentDate = DateTime.Now
                        };
                        await _assigmentService.AddAssignmentHistoryAsync(assignmentHistory);

                        currentAssignment.PreviusAssigmenId = currentAssignment.UserId;

                        if (model.SelectedUserId != null)
                        {
                            currentAssignment.UserId = model.SelectedUserId.Value;
                        }

                        currentAssignment.AssignmentDate = DateTime.Now;

                        var newUser = await _staffManager.GetStaffByIdAsync(model.SelectedUserId.Value);
                        if (newUser != null)
                        {
                            var toEmailAddress = newUser.Email;
                            var subject = "Yeni Ürün Ataması";
                            var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                                       $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                                       $"<p style='text-align: center;'>" +
                                       $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                                       $"</p>" +
                                       $"<p>Merhaba <strong>{newUser.Name}</strong>,</p>" +
                                       $"<p>Aşağıda belirtilen ürün zimmetinize eklenmiştir.</p>" +
                                       $"<p><strong>Ürün :</strong> {model.ProductBrand} {model.ProductModel}</p>" +
                                       $"<p><strong>Envanter Barkodu :</strong> {model.ProductBarkod}</p>" +
                                       $"<p>Teşekkürler</p>" +
                                       $"<p>İyi Çalışmalar</p>" +
                                       $"</div></body></html>";
                            var attachmentPaths = new List<string>();
                            await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
                        }
                        await _assigmentService.UpdateAssigmentAsync(currentAssignment);
                    }
                    else
                    {
                        await _productService.UpdateProductAsync(product);
                    }

                    TempData["SuccessMessage"] = "Ürün Başarıyla Güncellendi";
                    return RedirectToAction("ProductDetails", new { id = model.Id });
                }
            }
            else if (model.SelectedUserId.HasValue && model.SelectedUserId.Value != 0)
            {
                var newAssignment = new InventoryAssigment
                {
                    ProductId = product.Id,
                    UserId = model.SelectedUserId.Value,
                    AssignmentDate = DateTime.Now,
                    PreviusAssigmenId = null,
                };

                await _assigmentService.AddAssignmentAsync(newAssignment);

                var newUser = await _staffManager.GetStaffByIdAsync(model.SelectedUserId.Value);
                if (newUser != null)
                {
                    var toEmailAddress = newUser.Email;
                    var subject = "Yeni Ürün Ataması";
                    var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                               $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                               $"<p style='text-align: center;'>" +
                               $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                               $"</p>" +
                               $"<p>Merhaba <strong>{newUser.Name}</strong>,</p>" +
                               $"<p>Aşağıda belirtilen ürün zimmetinize eklenmiştir.</p>" +
                               $"<p><strong>Ürün :</strong> {model.ProductBrand} {model.ProductModel}</p>" +
                               $"<p><strong>Envanter Barkodu :</strong> {model.ProductBarkod}</p>" +
                               $"<p>Teşekkürler</p>" +
                               $"<p>İyi Çalışmalar</p>" +
                               $"</div></body></html>";
                    var attachmentPaths = new List<string>();
                    await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);
                }

                TempData["SuccessMessage"] = "Ürün Başarıyla Güncellendi";
                return RedirectToAction("ProductDetails", new { id = model.Id });
            }

            await _productService.UpdateProductAsync(product);
            TempData["SuccessMessage"] = "Ürün Başarıyla Güncellendi";

            return RedirectToAction("ProductDetails", new { id = model.Id });
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductEditViewModel model)
        {
            if (!await _productService.ProductBarkodUnique(model.ProductBarkod))
            {
                TempData["ErrorMessage"] = "Girdiğiniz barkod numarası başka ürüne ait";
                return RedirectToAction("ProductCreate", model);
            }

            if (model.SelectedCategoryId <= 0)
            {
                TempData["ErrorMessage"] = "Lütfen Kategori Seçiniz";
                return RedirectToAction("ProductCreate", model);
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                ProductBarkod = model.ProductBarkod,
                Description = model.Description,
                SerialNumber = model.SerialNumber,
                Categories = model.Categories,
                ProductBrand = model.ProductBrand,
                ProductModel = model.ProductModel
            };

            var result = await _productService.CreateProduct(product);

            if (result)
            {
                var productCategory = new ProductCategory
                {
                    ProductId = product.Id,
                    CategoryId = model.SelectedCategoryId
                };

                var toEmailAddress = "itsupport@joygame.com";
                var subject = "Envantere Yeni Ürün Eklendi!";
                var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                           $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                           $"<p style='text-align: center;'>" +
                           $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                           $"</p>" +
                           $"<p>Merhaba</p>" +
                           $"<p>Aşağıda belirtilen ürün Joygame Zimmetine eklenmiştir.</p>" +
                           $"<p><strong>Ürün :</strong> {model.ProductBrand} {model.ProductModel}</p>" +
                           $"<p><strong>Ürünün Seri Numarası :</strong> {model.SerialNumber}</p>" +
                           $"<p><strong>Envanter Barkodu :</strong> {model.ProductBarkod}</p>" +
                           $"<p>Teşekkürler</p>" +
                           $"<p>İyi Çalışmalar</p>" +
                           $"</div></body></html>";
                var attachmentPaths = new List<string>();
                await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);

                await _productService.AddProductCategory(productCategory);

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
            var product = await _productService.GetIdProductAsync(id);
            await _productService.DeleteProductAsync(id);

            var toEmailAddress = "itsupport@joygame.com";
            var subject = "Envanterden Ürün Çıkarılması Hakkında";
            var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                       $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                       $"<p style='text-align: center;'>" +
                       $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                       $"</p>" +
                       $"<p>Merhaba</p>" +
                       $"<p>Aşağıda belirtilen ürün Joygame Zimmetine eklenmiştir.</p>" +
                       $"<p><strong>Ürün :</strong> {product.ProductBrand} {product.ProductModel}</p>" +
                       $"<p><strong>Ürünün Seri Numarası :</strong> {product.SerialNumber}</p>" +
                       $"<p><strong>Envanter Barkodu :</strong> {product.ProductBarkod}</p>" +
                       $"<p>Teşekkürler</p>" +
                       $"<p>İyi Çalışmalar</p>" +
                       $"</div></body></html>";
            var attachmentPaths = new List<string>();
            await _emailService.SendEmailAsync(toEmailAddress, subject, body, attachmentPaths);

            TempData["SuccessMessage"] = "Ürün başarıyla silindi!";
            return RedirectToAction("ProductList");
        }
    }
}

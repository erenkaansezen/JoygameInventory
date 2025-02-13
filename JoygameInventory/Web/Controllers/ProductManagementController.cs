using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace JoygameInventory.Web.Controllers
{
    public class ProductManagementController : Controller
    {
        private readonly IProductService _productservice;
        private readonly IJoyStaffService _staffmanager;
        private readonly IAssigmentService _assigmentservice;
        private readonly EmailService _emailService;
        private readonly IMaintenanceService _maintenanceservice;

        public ProductManagementController(IProductService productservice, IJoyStaffService staffmanager, IAssigmentService assigmentService, EmailService emailService, IMaintenanceService maintenanceservice)
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

            if (!string.IsNullOrEmpty(searchTerm))
            {
                joyproducts = await _productservice.SearchProduct(searchTerm); 
            }

            if (!string.IsNullOrEmpty(category))
            {
                joyproducts = await _productservice.GetProductsByCategoryAsync(category); 
            }


            return View(joyproducts);
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var staff = await _productservice.GetIdProductAsync(id);
            if (staff != null)
            {
                var maintenance = await _maintenanceservice.GetProductServiceAsync(staff.ProductBarkod);
                var maintenanceHistory = await _maintenanceservice.GetProductServiceHistoryAsync(staff.ProductBarkod);
                var inventoryAssignments = await _assigmentservice.GetProductAssignmentsAsync(staff.Id);
                var productcategory = await _productservice.GetProductCategoryAsync(staff.Id);
                var previousAssignments = await _assigmentservice.GetPreviousAssignmentsAsync(staff.Id);
                var assignmentHistorys = await _assigmentservice.GetAssignmentHistoryAsync(id);
                var category = await _productservice.GetAllCategoriesAsync();

                var joystaff = await _staffmanager.GetAllStaffsAsync();
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
                    Storage= staff.Storage,
                    Status = staff.Status,
                    InventoryAssigments = inventoryAssignments,
                    JoyStaffs = joystaff,
                    AssigmentHistorys = assignmentHistorys,
                    Categories = category,
                    MaintenanceHistorys = maintenanceHistory,
                    Maintenance = maintenance,
                    ProductCategory = productcategory,




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



            var currentAssignments = await _assigmentservice.GetProductAssignmentsAsync(product.Id);

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
                        await _assigmentservice.AddAssignmentHistoryAsync(assignmentHistory);

                        currentAssignment.PreviusAssigmenId = currentAssignment.UserId;

                        if (model.SelectedUserId != null)
                        {
                            currentAssignment.UserId = model.SelectedUserId.Value;
                        }

                        currentAssignment.AssignmentDate = DateTime.Now;

                        var newUser = await _staffmanager.GetStaffByIdAsync(model.SelectedUserId.Value);
                        if (newUser != null)
                        {
                            var toEmailAddress = newUser.Email;
                            var subject = "Yeni Ürün Ataması";
                            var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                                       $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                                       $"<p style='text-align: center;'>" +  // Sadece bu satırda text-align: center; kullanarak resmin ortalanmasını sağlıyoruz
                                       $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                                       $"</p>" +  // Fotoğrafı bir <p> içine alıp, sadece onu ortalamış olduk.
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
                        await _assigmentservice.UpdateAssigmentAsync(currentAssignment);
                    }
                    else
                    {
                        await _productservice.UpdateProductAsync(product);
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

                await _assigmentservice.AddAssignmentAsync(newAssignment);
                
                var newUser = await _staffmanager.GetStaffByIdAsync(model.SelectedUserId.Value);
                if (newUser != null)
                {
                    var toEmailAddress = newUser.Email;
                    var subject = "Yeni Ürün Ataması";
                    var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                               $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                               $"<p style='text-align: center;'>" +  // Sadece bu satırda text-align: center; kullanarak resmin ortalanmasını sağlıyoruz
                               $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                               $"</p>" +  // Fotoğrafı bir <p> içine alıp, sadece onu ortalamış olduk.
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



            await _productservice.UpdateProductAsync(product);
            TempData["SuccessMessage"] = "Ürün Başarıyla Güncellendi";

            return RedirectToAction("ProductDetails", new { id = model.Id });
        }



        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductEditViewModel model)
        {
            if (!await _productservice.ProductBarkodUnique(model.ProductBarkod))
            {
                TempData["ErrorMessage"] = "Girdiğiniz barkod numarası başka ürüne ait";
                return View("ProductCreate", model);
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



            var result = await _productservice.CreateProduct(product);

            if (result) 
            {
                var productCategory = new ProductCategory
                {
                    ProductId = product.Id, // Ürün ID'si
                    CategoryId = model.SelectedCategoryId // Seçilen kategori ID'si
                };

                    var toEmailAddress = "itsupport@joygame.com";
                    var subject = "Envantere Yeni Ürün Eklendi!";
                    var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                               $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                               $"<p style='text-align: center;'>" +  // Sadece bu satırda text-align: center; kullanarak resmin ortalanmasını sağlıyoruz
                               $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                               $"</p>" +  // Fotoğrafı bir <p> içine alıp, sadece onu ortalamış olduk.
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
            var product = await _productservice.GetIdProductAsync(id);
            await _productservice.DeleteProductAsync(id);

            var toEmailAddress = "itsupport@joygame.com";
            var subject = "Envanterden Ürün Çıkarılması Hakkında";
            var body = $"<html><head></head><body style='font-family: Arial, sans-serif; background-color: #f4f4f4;'>" +
                       $"<div style='margin: 20px; background-color: white; padding: 20px; text-align: center'>" +
                       $"<p style='text-align: center;'>" +  // Sadece bu satırda text-align: center; kullanarak resmin ortalanmasını sağlıyoruz
                       $"<img src='https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQckotOde3DMZ24VrcgME7-tMTF_FcQvODrbQ&s' alt='Ürün Fotoğrafı' style='max-width: 50%; height: auto;'/>" +
                       $"</p>" +  // Fotoğrafı bir <p> içine alıp, sadece onu ortalamış olduk.
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
            return RedirectToAction("ProductList", "ProductManagement");
        }


    }
}

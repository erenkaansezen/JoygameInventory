using JoygameInventory.Business.Services;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace JoygameInventory.Web.Controllers
{
    public class ProductManagementController : Controller
    {
        public ProductService _productservice;
        public JoyStaffService _staffmanager;
        public AssigmentService _assigmentservice;

        public ProductManagementController(ProductService productservice, JoyStaffService staffmanager,AssigmentService assigmentService)
        {
            _productservice = productservice;
            _staffmanager = staffmanager;
            _assigmentservice = assigmentService;
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
            var products = await _productservice.GetIdProductAsync(model.Id);
            if (products != null)
            {
                // Ürün bilgilerini güncelliyoruz
                products.ProductName = model.ProductName;
                products.SerialNumber = model.SerialNumber;
                products.ProductBarkod = model.ProductBarkod;
                products.Description = model.Description;
                products.ProductAddDate = DateTime.Now;

                // Mevcut atama kaydını alıyoruz
                var currentAssignments = await _assigmentservice.GetProductAssignmentsAsync(products.Id);

                if (currentAssignments != null && currentAssignments.Any())
                {
                    var currentAssignment = currentAssignments.FirstOrDefault();

                    if (currentAssignment != null)
                    {


                        // Mevcut atama varsa ve kullanıcı değişmişse
                        if (currentAssignment.UserId != model.SelectedUserId && model.SelectedUserId != null)
                        {
                            // Önceki kullanıcıyı AssignmentHistory tablosuna kaydediyoruz
                            var assignmentHistory = new AssigmentHistory
                            {
                                ProductId = currentAssignment.ProductId,
                                UserId = currentAssignment.UserId,  // Eski kullanıcı ID'si
                                AssignmentDate = DateTime.Now
                            };
                            await _assigmentservice.AddAssignmentHistoryAsync(assignmentHistory);  // AssignmentHistory kaydını ekliyoruz

                            // Önceki kullanıcıyı PreviusAssigmenId'ye kaydediyoruz
                            currentAssignment.PreviusAssigmenId = currentAssignment.UserId;
                            if (model.SelectedUserId != null)
                            {
                                currentAssignment.UserId = model.SelectedUserId.Value;

                            }

                            // Atama tarihini güncelliyoruz
                            currentAssignment.AssignmentDate = DateTime.Now;

                            // Atama kaydını güncelliyoruz
                            await _assigmentservice.UpdateAssigmentAsync(currentAssignment);
                        }
                        else
                        {
                            // Kullanıcı değişmemişse sadece ürünü güncelliyoruz
                            await _productservice.UpdateProductAsync(products);
                        }

                        return RedirectToAction("ProductDetails", new { id = model.Id });
                    }
                }
                else if (model.SelectedUserId.HasValue && model.SelectedUserId.Value != 0) // Atama kaydı yok ve kullanıcı seçildiyse
                {
                    // Yeni bir atama kaydı oluşturuyoruz
                    var newAssignment = new InventoryAssigment
                    {
                        ProductId = products.Id,
                        UserId = model.SelectedUserId.Value,  // Seçilen kullanıcı
                        AssignmentDate = DateTime.Now,
                        PreviusAssigmenId = null,  // null yapılıyor
                    };

                    // Yeni atamayı kaydediyoruz
                    await _assigmentservice.AddAssignmentAsync(newAssignment);

                    return RedirectToAction("ProductDetails", new { id = model.Id });
                }

                // Ürün bilgilerini güncelliyoruz
                await _productservice.UpdateProductAsync(products);

                return RedirectToAction("ProductDetails", new { id = model.Id });
            }

            return RedirectToAction("Index", "Home");
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

        [HttpPost]
        public async Task<IActionResult> AssigmentDelete(int userId, int inventoryAssigmentId)
        {
            // Kullanıcıyı veritabanından alıyoruz
            var staff = await _staffmanager.GetStaffByIdAsync(userId);
            var assigment = await _assigmentservice.GetAssignmentByIdAsync(inventoryAssigmentId);

            if (assigment != null)
            {

                // AssigmentHistory kaydını oluşturuyoruz
                var assignmentHistory = new AssigmentHistory
                {

                    ProductId = assigment.ProductId,
                    UserId = userId,
                    AssignmentDate = DateTime.Now,


                };
                await _assigmentservice.AddAssignmentHistoryAsync(assignmentHistory);  // AssignmentHistory kaydını ekliyoruz
            }

            await _assigmentservice.DeleteAssignmentAsync(inventoryAssigmentId);

            // Silme işlemi başarılı, kullanıcı detaylarına yönlendirelim
            return RedirectToAction("StaffDetails", "StaffManagement", new { id = userId });
        }
    }
}

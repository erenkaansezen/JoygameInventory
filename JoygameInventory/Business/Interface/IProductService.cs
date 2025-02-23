using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoygameInventory.Business.Services
{
    public interface IProductService
    {
        // Tüm ürünleri al
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<IEnumerable<Product>> GetProductAsync(string searchTerm,string category);


        // Ürünü ID ile al
        Task<Product> GetIdProductAsync(int id);

        // Belirli bir kategoriye ait ürünleri al
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryUrl);

        //Product Details
        Task<ProductEditViewModel> GetProductDetailsAsync(int id);

        // Ürünü oluşturmak için view modelini al
        Task<ProductEditViewModel> GetCreateViewAsync(); // GET için metot


        // Yeni bir ürün ekle
        Task<bool> CreateProduct(Product product);

        // Tüm kategorileri al
        Task<List<Category>> GetAllCategoriesAsync();

        // Ürün kategorisini ID ile al
        Task<IEnumerable<ProductCategory>> GetProductCategoryAsync(int categoryId);

        // Tüm ürün kategorilerini al
        Task<IEnumerable<ProductCategory>> GetAllProductCategoriesAsync();

        // Ürün kategorisi ekle
        Task AddProductCategory(ProductCategory productCategory);

        // Ürünü güncelle
        Task UpdateProductAsync(Product product);

        // Ürünü sil
        Task DeleteProductAsync(int id);

        // Ürün araması yap
        Task<IEnumerable<Product>> SearchProduct(string searchTerm);

        // Ürün barkodunun benzersiz olup olmadığını kontrol et
        Task<bool> ProductBarkodUnique(string barkod);

        // Kategoriye ait ürünü al
        Product GetCategoryById(int id);

        Task<int?> GetCurrentCategoryIdAsync(int productId);

        Task UpdateProductCategoryAsync(int productId, int selectedCategoryId);

    }
}

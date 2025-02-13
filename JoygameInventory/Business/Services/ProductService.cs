using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly InventoryContext _context;

        public ProductService(InventoryContext context)
        {
            _context = context;
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

        public async Task<bool> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
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
        public async Task AddProductCategory(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
             await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var deleteProduct = await _context.Products.FindAsync(id);
            if (deleteProduct != null)
            {
                _context.Products.Remove(deleteProduct);
                await _context.SaveChangesAsync();
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
        public async Task UpdateProductCategoryAsync(ProductCategory product, int newCategoryId)
        {
                product.CategoryId = newCategoryId;
                _context.ProductCategories.Update(product);
                await _context.SaveChangesAsync();
        }
        public async Task UpdateProductCategoryAsync(int productId, int selectedCategoryId)
        {
            // Ürünü alıyoruz
            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(pc => pc.ProductId == productId); // Burada ProductId ile ilişkilendiriyoruz

            if (productCategory != null)
            {
                // Kategoriyi güncelliyoruz
                productCategory.CategoryId = selectedCategoryId;

                // Değişiklikleri kaydediyoruz
                await _context.SaveChangesAsync();
            }
        }


    }
}

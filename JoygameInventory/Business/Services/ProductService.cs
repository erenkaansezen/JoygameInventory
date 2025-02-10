using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class ProductService
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
                // Eğer ürünün barkodu Maintenance tablosunda varsa
                var maintenance = await _context.Maintenance
                    .FirstOrDefaultAsync(m => m.ProductBarkod == product.ProductBarkod);

                // Barkod bulunduyse ve servisteyse durumu "Servis" olarak ayarla
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
                .Include(ia => ia.Product)  // Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.ProductId == categoryId)  // Ürüne ait zimmetli envanterleri al
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
    }
}

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
                .Include(p => p.InventoryAssigments)  // InventoryAssigments ile birlikte yükle
                .ThenInclude(ia => ia.User)  // InventoryAssigments'teki AssignedUser bilgisini de yükle
                .ToList();
            foreach (var product in products)
            {
                if (product.InventoryAssigments == null || !product.InventoryAssigments.Any())
                {
                    product.Status = "Depoda";
                }
                else
                {
                    product.Status = "Zimmetli";  // Eğer InventoryAssigments varsa, ürün zimmetli kabul edilir
                }
            }

            return products;
        }
        public async Task<Product> GetIdProductAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(s => s.Id == id);

        }
        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }

}

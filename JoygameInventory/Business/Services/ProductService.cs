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
        public async Task<Product> GetIdProductAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(s => s.Id == id);

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

    }
}

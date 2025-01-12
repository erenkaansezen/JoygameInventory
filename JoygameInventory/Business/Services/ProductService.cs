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
            return  _context.Products.ToList();
        }

    }

}

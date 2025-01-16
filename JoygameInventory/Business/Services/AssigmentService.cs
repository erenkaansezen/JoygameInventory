using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class AssigmentService
    {
        private readonly InventoryContext _context;

        public AssigmentService(InventoryContext context)
        {
            _context = context;
        }

        
        public async Task<IEnumerable<InventoryAssigment>> GetUserAssignmentsAsync(int userId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Include(ia => ia.Product)
                .Include(ia => ia.User)// Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.UserId == userId)  // Kullanıcıya ait zimmetli envanterleri al
                .ToListAsync();

            return inventoryAssignments;
        }
        public async Task<IEnumerable<InventoryAssigment>> GetProductAssignmentsAsync(int ProductId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Include(ia => ia.Product)
                .Include(ia => ia.User)// Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.ProductId == ProductId)  // Kullanıcıya ait zimmetli envanterleri al
                .ToListAsync();

            return inventoryAssignments;
        }
        public async Task<List<InventoryAssigment>> GetPreviousAssignmentsAsync(int productId)
        {
            // Ürünle ilgili geçmişteki tüm atamaları alıyoruz
            var previousAssignments = await _context.InventoryAssigments
                .Where(pa => pa.ProductId == productId)
                .OrderByDescending(pa => pa.AssignmentDate) // Atama tarihine göre sıralıyoruz
                .Skip(1) // En son atamayı atlıyoruz (yani önceki atamaları alıyoruz)
                .ToListAsync();  // Veritabanından liste olarak alıyoruz

            return previousAssignments;
        }
        public async Task UpdateProductAsync(InventoryAssigment assigment)
        {
            _context.InventoryAssigments.Update(assigment);
            await _context.SaveChangesAsync();
        }

    }

}

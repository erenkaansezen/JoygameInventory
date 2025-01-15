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

        
        public async Task<IEnumerable<InventoryAssigment>> GetInventoryAssignmentsAsync(string userId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Include(ia => ia.Product)  // Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.UserId == userId)  // Kullanıcıya ait zimmetli envanterleri al
                .ToListAsync();

            return inventoryAssignments;
        }

    }

}

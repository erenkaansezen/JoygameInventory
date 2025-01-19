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

        // Kullanıcıya ait envanter atamaları
        public async Task<IEnumerable<InventoryAssigment>> GetUserAssignmentsAsync(int userId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Include(ia => ia.Product)
                .Include(ia => ia.User)  // Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.UserId == userId)  // Kullanıcıya ait zimmetli envanterleri al
                .ToListAsync();

            return inventoryAssignments;
        }

        // Ürüne ait envanter atamaları
        public async Task<IEnumerable<InventoryAssigment>> GetProductAssignmentsAsync(int productId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Include(ia => ia.Product)
                .Include(ia => ia.User)  // Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.ProductId == productId)  // Ürüne ait zimmetli envanterleri al
                .ToListAsync();

            return inventoryAssignments;
        }

        // Ürüne ait geçmişteki tüm atamaları alıyoruz
        public async Task<List<InventoryAssigment>> GetPreviousAssignmentsAsync(int productId)
        {
            var previousAssignments = await _context.InventoryAssigments
                .Where(pa => pa.ProductId == productId && pa.PreviusAssigmenId != 0)  // Önceki atamalar
                .OrderByDescending(pa => pa.AssignmentDate) // Atama tarihine göre sıralıyoruz
                .ToListAsync();  // Veritabanından liste olarak alıyoruz

            return previousAssignments;
        }

        // PreviusAssigmentUser ile eşleşen eski kullanıcı bilgilerini almak için düzenlenmiş metod
        public async Task<JoyStaff> GetPreviousUserByIdAsync(int previousUserId)
        {
            // PreviusAssigmentUser id'si ile eşleşen kullanıcıyı JoyStaff tablosundan çekiyoruz
            var previousUser = await _context.JoyStaffs
                .Where(user => user.Id == previousUserId)
                .FirstOrDefaultAsync();  // Kullanıcıyı getiriyoruz

            return previousUser;
        }

        // Ürün atamasını güncellemek veya yeni atama eklemek
        public async Task UpdateProductAsync(InventoryAssigment newAssignment)
        {
            // Mevcut atama kaydını buluyoruz
            var currentAssignment = await _context.InventoryAssigments
                .Where(ia => ia.ProductId == newAssignment.ProductId && ia.UserId == newAssignment.UserId)
                .FirstOrDefaultAsync();

            if (currentAssignment != null)
            {
                // Eğer eski atama varsa, PreviusAssigmentUser'ı güncelliyoruz
                currentAssignment.PreviusAssigmenId = currentAssignment.UserId;

                // Yeni kullanıcıyı güncelliyoruz
                currentAssignment.UserId = newAssignment.UserId; // Yeni kullanıcı id'sini atıyoruz
                currentAssignment.AssignmentDate = DateTime.Now; // Yeni atama tarihini güncelliyoruz

                // Eski atama kaydını güncelliyoruz
                _context.InventoryAssigments.Update(currentAssignment);
                await _context.SaveChangesAsync();
            }
        }
    }
}

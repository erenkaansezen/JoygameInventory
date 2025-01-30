using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
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
        public async Task<List<Category>> GetAllStaffsAsync()
        {
            // Veritabanından tüm staff'leri asenkron şekilde çekiyoruz
            return await _context.Categories.ToListAsync();
        }

        public async Task<InventoryAssigment> GetAssignmentByIdAsync(int inventoryAssigmentId)
        {
            var inventoryAssignments = await _context.InventoryAssigments
                .Where(a => a.Id == inventoryAssigmentId)
                .FirstOrDefaultAsync();
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


        // InventoryAssigment güncellenmesi için metod
        public async Task UpdateAssigmentAsync(InventoryAssigment assignment)
        {
            if (assignment == null)
            {
                throw new ArgumentNullException(nameof(assignment), "Atama verisi boş olamaz.");
            }

            // Veritabanındaki mevcut atamayı alıyoruz
            var existingAssignment = await _context.InventoryAssigments
                .FirstOrDefaultAsync(a => a.Id == assignment.Id);



            // Mevcut atamayı güncelliyoruz
            existingAssignment.UserId = assignment.UserId;
            existingAssignment.AssignmentDate = assignment.AssignmentDate;

            // Güncellenen atamayı kaydediyoruz
            _context.InventoryAssigments.Update(existingAssignment);
            await _context.SaveChangesAsync();
        }

        public async Task AddAssignmentAsync(InventoryAssigment newAssignment)
        {

                // Veritabanına ekliyoruz
                _context.InventoryAssigments.Add(newAssignment);

                // Veritabanındaki değişiklikleri kaydediyoruz
                await _context.SaveChangesAsync();
            

        }
        public async Task DeleteAssignmentAsync(int id)
        {
            var deleteAssigment = await _context.InventoryAssigments.FindAsync(id);
            if (deleteAssigment != null)
            {
                _context.InventoryAssigments.Remove(deleteAssigment);
                await _context.SaveChangesAsync();
            }
        }

        //History Service 

        public async Task AddAssignmentHistoryAsync(AssigmentHistory assignmentHistory)
        {
            // İlgili ürünün atama geçmişini alıyoruz
            var assignmentHistories = await _context.AssigmentHistorys
                                                      .Where(a => a.ProductId == assignmentHistory.ProductId)
                                                      .OrderByDescending(a => a.AssignmentDate) // Tarihe göre sıralama
                                                      .ToListAsync();

            // Eğer 5'ten fazla kayıttan fazla varsa, en eski kaydı siliyoruz
            if (assignmentHistories.Count >= 5)
            {
                // En eski kaydı sil
                var oldestHistory = assignmentHistories.Last();
                _context.AssigmentHistorys.Remove(oldestHistory);
            }

            // Yeni atama geçmişini ekliyoruz
            _context.AssigmentHistorys.Add(assignmentHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AssigmentHistory>> GetAssignmentHistoryAsync(int productId)
        {
            // Ürüne ait atama geçmişlerini getiriyoruz
            return await _context.AssigmentHistorys
                                 .Where(a => a.ProductId == productId)
                                 .OrderByDescending(a => a.AssignmentDate)  // Son yapılan atamayı en önce göstermek için
                                 .ToListAsync();
        }
    }
}

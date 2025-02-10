using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoygameInventory.Business.Services
{
    public interface IAssigmentService
    {
        // Kullanıcıya ait envanter atamaları
        Task<IEnumerable<InventoryAssigment>> GetUserAssignmentsAsync(int userId);

        // Ürüne ait envanter atamaları
        Task<IEnumerable<InventoryAssigment>> GetProductAssignmentsAsync(int productId);

        // Tüm staff'leri al
        Task<List<Category>> GetAllStaffsAsync();

        // Atama ID'sine göre envanter atamasını al
        Task<InventoryAssigment> GetAssignmentByIdAsync(int inventoryAssigmentId);

        // Ürüne ait geçmişteki tüm atamaları al
        Task<List<InventoryAssigment>> GetPreviousAssignmentsAsync(int productId);

        // PreviusAssigmentUser ile eşleşen eski kullanıcı bilgilerini al
        Task<JoyStaff> GetPreviousUserByIdAsync(int previousUserId);

        // Envanter atamasını güncelle
        Task UpdateAssigmentAsync(InventoryAssigment assignment);

        // Yeni bir envanter ataması ekle
        Task AddAssignmentAsync(InventoryAssigment newAssignment);

        // Envanter atamasını sil
        Task DeleteAssignmentAsync(int id);

        // Atama geçmişi ekle
        Task AddAssignmentHistoryAsync(AssigmentHistory assignmentHistory);

        // Ürüne ait atama geçmişlerini al
        Task<List<AssigmentHistory>> GetAssignmentHistoryAsync(int productId);
    }
}

using JoygameInventory.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoygameInventory.Business.Services
{
    public interface IJoyStaffService
    {
        // Tüm staff'leri al
        Task<List<JoyStaff>> GetAllStaffsAsync();

        // ID'ye göre tek bir staff'ı al
        Task<JoyStaff> GetStaffByIdAsync(int id);

        // Yeni bir staff oluştur
        Task<bool> CreateStaff(JoyStaff staff);

        // Staff'ı güncelle
        Task<bool> UpdateStaffAsync(JoyStaff staff);

        // Staff'ı sil
        Task DeleteStaffAsync(int id);

        // E-posta benzersiz mi kontrol et
        Task<bool> IsEmailUnique(string email);

        // Staff'ları arama yaparak listele
        Task<IEnumerable<JoyStaff>> SearchStaff(string searchTerm);

        // Panel kullanıcıları için e-posta benzersiz mi kontrol et
        Task<bool> PanelIsEmailUnique(string email);

        // Panel kullanıcılarını arama yaparak listele
        Task<IEnumerable<JoyUser>> SearchPanelStaff(string searchTerm);
    }
}

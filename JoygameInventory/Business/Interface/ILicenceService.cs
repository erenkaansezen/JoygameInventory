using JoygameInventory.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoygameInventory.Business.Services
{
    public interface ILicenceService
    {
        // Tüm lisansları al
        Task<List<Licence>> GetAllLicencesAsync();

        // Lisans adını arama yaparak lisansları al
        Task<IEnumerable<Licence>> SearchLicence(string searchTerm);

        // Yeni bir lisans ekle
        Task<bool> AddLicence(Licence licence);

        // ID'ye göre lisans al
        Task<Licence> GetLicenceByIdAsync(int id);

        // Kullanıcıya ait lisans atamalarını al
        Task<List<LicenceUser>> GetLicenceUserAssignmentsAsync(int userId);

        // Staff'a ait lisans atamalarını al
        Task<List<LicenceUser>> GetUserLicenceAssignmentsAsync(int userId);

        // Lisans atamasını ID'ye göre al
        Task<LicenceUser> GetAssignmentByIdAsync(int licenceAssigmentId);

        // Lisans atamasını sil
        Task DeleteLicenceAssigmentAsync(int id);

        // Lisans ataması ekle
        Task AddAssignmentAsync(Licence licence, JoyStaff staff);

        // Lisansı sil
        Task DeleteLicenceAsync(int id);

        // Lisans adı benzersiz mi kontrol et
        Task<bool> IsLicenceUnique(string licenceName);
    }
}

using JoygameInventory.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoygameInventory.Business.Services
{
    public interface IMaintenanceService
    {
        // Tüm bakım hizmetlerini al
        Task<List<Maintenance>> GetAllServiceAsync();

        // Ürün barkoduna göre bakım hizmetini al
        Task<Maintenance> GetProductServiceAsync(string productBarkod);

        // Ürün barkoduna göre bakım geçmişini al
        Task<IEnumerable<MaintenanceHistory>> GetProductServiceHistoryAsync(string productBarkod);

        // Bakım hizmetlerini arama yaparak al
        Task<IEnumerable<Maintenance>> SearchMaintenanceService(string searchTerm);

        // ID'ye göre bakım hizmetini al
        Task<Maintenance> GetMaintenanceByIdAsync(int id);

        // Bakım geçmişi ekle
        Task<bool> MaintenanceHistoryAdd(MaintenanceHistory maintenance);

        // Yeni bir bakım hizmeti ekle
        Task<bool> CreateMaintenance(Maintenance maintenance);

        // Bakım hizmetini sil
        Task DeleteMaintenanceAsync(int id);

        // Ürün barkodu benzersiz mi kontrol et
        Task<bool> IsProductBarkodUnique(string productBarkod);
    }
}

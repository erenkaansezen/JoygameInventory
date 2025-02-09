using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class MaintenanceService
    {
        private readonly InventoryContext _context;

        // Constructor ile context'i alıyoruz
        public MaintenanceService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<Maintenance>> GetAllServiceAsync()
        {
            // Veritabanından tüm staff'leri asenkron şekilde çekiyoruz
            return await _context.Maintenance.ToListAsync();
        }

        public async Task<IEnumerable<Maintenance>> SearchMaintenanceService(string searchTerm)
        {
            var query = _context.Maintenance.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(Maintenance => EF.Functions.Like(Maintenance.ProductBarkod, "%" + searchTerm + "%"));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> CreateMaintenance(Maintenance maintenance)
        {
            try
            {
                // Servisi veritabanına ekle
                _context.Maintenance.Add(maintenance);
                await _context.SaveChangesAsync();
                return true;  // Başarılı
            }
            catch (Exception)
            {
                // Hata durumunda false döner
                return false;
            }
        }
    }
}

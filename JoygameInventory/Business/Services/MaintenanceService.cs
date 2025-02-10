using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class MaintenanceService : IMaintenanceService
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
        public async Task<Maintenance> GetProductServiceAsync(string ProductBarkod)
        {
            try
            {
                var inventoryAssignment = await _context.Maintenance
                    .Where(ia => ia.ProductBarkod == ProductBarkod)
                    .FirstOrDefaultAsync();

                if (inventoryAssignment == null)
                {
                    // Loglama veya hata mesajı ekleyebilirsiniz
                    Console.WriteLine($"No maintenance record found for ProductBarkod: {ProductBarkod}");
                }

                return inventoryAssignment;
            }
            catch (Exception ex)
            {
                // Hata loglaması
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetProductServiceHistoryAsync(string ProductBarkod)
        {
            var inventoryAssignments = await _context.MaintenanceHistory
                .Where(ia => ia.ProductBarkod == ProductBarkod)  // ProductBarkod'a göre filtreleme
                .ToListAsync();

            return inventoryAssignments;
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

        public async Task<Maintenance> GetMaintenanceByIdAsync(int id)
        {
            // Veritabanında belirtilen id'ye sahip bakımı buluyoruz.
            var maintenance = await _context.Maintenance
                                            .FirstOrDefaultAsync(m => m.Id == id);

            return maintenance;
        }

        public async Task<bool> MaintenanceHistoryAdd(MaintenanceHistory maintenance)
        {
            try
            {
                // Servisi veritabanına ekle
                _context.MaintenanceHistory.Add(maintenance);
                await _context.SaveChangesAsync();
                return true;  // Başarılı
            }
            catch (Exception ex)
            {
                return false;
            }
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
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task DeleteMaintenanceAsync(int id)
        {
            var deleteMaintenance = await _context.Maintenance.FindAsync(id);
            if (deleteMaintenance != null)
            {
                _context.Maintenance.Remove(deleteMaintenance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsProductBarkodUnique(string ProductBarkod)
        {
            return !await _context.Maintenance.AnyAsync(s => s.ProductBarkod == ProductBarkod);
        }



    }
}

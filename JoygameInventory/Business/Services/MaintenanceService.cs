using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class MaintenanceService : IMaintenanceService
    {
        private readonly InventoryContext _context;

        public MaintenanceService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<Maintenance>> GetAllServiceAsync()
        {
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
                    Console.WriteLine($"No maintenance record found for ProductBarkod: {ProductBarkod}");
                }

                return inventoryAssignment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }

        public async Task<IEnumerable<MaintenanceHistory>> GetProductServiceHistoryAsync(string ProductBarkod)
        {
            var inventoryAssignments = await _context.MaintenanceHistory
                .Where(ia => ia.ProductBarkod == ProductBarkod)  
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
            var maintenance = await _context.Maintenance
                                            .FirstOrDefaultAsync(m => m.Id == id);

            return maintenance;
        }

        public async Task<bool> MaintenanceHistoryAdd(MaintenanceHistory maintenance)
        {
            try
            {
                _context.MaintenanceHistory.Add(maintenance);
                await _context.SaveChangesAsync();
                return true;  
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
                _context.Maintenance.Add(maintenance);
                await _context.SaveChangesAsync();
                return true; 
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

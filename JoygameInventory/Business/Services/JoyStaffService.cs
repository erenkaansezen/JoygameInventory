using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{

        public class JoyStaffService
        {
            private readonly InventoryContext _context;

            // Constructor ile context'i alıyoruz
            public JoyStaffService(InventoryContext context)
            {
                _context = context;
            }

            // Tüm staff'leri listelemek için bir metod
            public async Task<List<JoyStaff>> GetAllStaffsAsync()
            {
                // Veritabanından tüm staff'leri asenkron şekilde çekiyoruz
                return await _context.JoyStaffs.ToListAsync();
            }

            // Tek bir staff'ı ID'ye göre getiren bir metod
            public async Task<JoyStaff> GetStaffByIdAsync(string id)
            {
            // Veritabanından staff'ı ID ile arıyoruz
            return await _context.JoyStaffs.FirstOrDefaultAsync(s => s.Id == id);
            }
        }

    }


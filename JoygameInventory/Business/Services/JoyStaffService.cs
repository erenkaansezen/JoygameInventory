using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{

    public class JoyStaffService : IJoyStaffService
    {
        private readonly InventoryContext _context;

        public JoyStaffService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<JoyStaff>> GetAllStaffsAsync()
        {
            return await _context.JoyStaffs.ToListAsync();
        }

        public async Task<JoyStaff> GetStaffByIdAsync(int id)
        {
            return await _context.JoyStaffs.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<bool> CreateStaff(JoyStaff staff)
        {
             _context.JoyStaffs.Add(staff);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateStaffAsync(JoyStaff staff)
        {
            _context.JoyStaffs.Update(staff);
            var result = await _context.SaveChangesAsync();
            return result > 0; 

        }
        public async Task DeleteStaffAsync(int id)
        {
            var deleteStaff = await _context.JoyStaffs.FindAsync(id);
            if (deleteStaff != null)
            {
                _context.JoyStaffs.Remove(deleteStaff);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsEmailUnique(string email)
        {
            return !await _context.JoyStaffs.AnyAsync(s => s.Email == email);
        }
        public async Task<IEnumerable<JoyStaff>> SearchStaff(string searchTerm)
        {
            var query = _context.JoyStaffs.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(staff => EF.Functions.Like(staff.Name, "%" + searchTerm + "%") ||
                                              EF.Functions.Like(staff.Surname, "%" + searchTerm + "%"));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> PanelIsEmailUnique(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }

            return !await _context.JoyUsers.AnyAsync(s => s.Email == email);
        }
        public async Task<IEnumerable<JoyUser>> SearchPanelStaff(string searchTerm)
        {
            var query = _context.JoyUsers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(panelUser => EF.Functions.Like(panelUser.UserName, "%" + searchTerm + "%") ||
                                              EF.Functions.Like(panelUser.Email, "%" + searchTerm + "%"));
            }

            return await query.ToListAsync();
        }
    }

}


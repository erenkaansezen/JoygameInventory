using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class LicenceService : ILicenceService
    {

        private readonly InventoryContext _context;

        public LicenceService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<Licence>> GetAllLicencesAsync()
        {
            return await _context.Licence.ToListAsync();
        }
        public async Task<IEnumerable<Licence>> SearchLicence(string searchTerm)
        {
            var query = _context.Licence.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(Licence => EF.Functions.Like(Licence.LicenceName, "%" + searchTerm + "%"));
            }

            return await query.ToListAsync();
        }
        public async Task<bool> AddLicence(Licence Licence)
        {
            _context.Licence.Add(Licence);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Licence> GetLicenceByIdAsync(int id)
        {
            return  _context.Licence.FirstOrDefault(s => s.Id == id);
        }
        public async Task<List<LicenceUser>> GetLicenceUserAssignmentsAsync(int userId)
        {
            var licenceusers = await _context.LicenceUser
                .Include(ia => ia.Licence)
                .Include(ia => ia.staff)  
                .Where(ia => ia.LicenceId == userId)  
                .ToListAsync();

            return licenceusers;
        }
        public async Task<List<LicenceUser>> GetUserLicenceAssignmentsAsync(int userId)
        {
            var licenceusers = await _context.LicenceUser
                .Include(ia => ia.Licence)
                .Include(ia => ia.staff)  
                .Where(ia => ia.StaffId == userId) 
                .ToListAsync();

            return licenceusers;
        }
        public async Task<LicenceUser> GetAssignmentByIdAsync(int LicenceAssigmentId)
        {
            var licenceAssigment = await _context.LicenceUser
                .Where(a => a.Id == LicenceAssigmentId)
                .FirstOrDefaultAsync();
            return licenceAssigment;
        }
        public async Task DeleteLicenceAssigmentAsync(int id)
        {
            var deleteAssigment = await _context.LicenceUser.FindAsync(id);
            if (deleteAssigment != null)
            {
                _context.LicenceUser.Remove(deleteAssigment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddAssignmentAsync(Licence licence, JoyStaff staff)
        {
            var assignment = new LicenceUser
            {
                LicenceId = licence.Id,
                StaffId = staff.Id,
            };

            _context.LicenceUser.Add(assignment);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteLicenceAsync(int id)
        {
            var deleteLicence = await _context.Licence.FindAsync(id);
            if (deleteLicence != null)
            {
                _context.Licence.Remove(deleteLicence);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<bool> IsLicenceUnique(string licenceName)
        {
            return !await _context.Licence.AnyAsync(s => s.LicenceName == licenceName);
        }
    }
}

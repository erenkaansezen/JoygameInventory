using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace JoygameInventory.Business.Services
{
    public class LicenceService : ILicenceService
    {

        private readonly InventoryContext _context;
        private readonly IJoyStaffService _staffManager;    

        public LicenceService(InventoryContext context,IJoyStaffService staffManager)
        {
            _context = context;
            _staffManager = staffManager;
        }

        public async Task<List<Licence>> GetAllLicencesAsync()
        {
            return await _context.Licence.ToListAsync();
        }
        public async Task<IEnumerable<Licence>> GetLicenceAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return await GetAllLicencesAsync();
            }
            return await SearchLicence(searchTerm);
        }

        public async Task<LicenceEditViewModel> GetLicenceDetailsAsync(int id)
        {
            var licence = await GetLicenceByIdAsync(id);
            var joyStaff = await _staffManager.GetAllStaffsAsync();
            var licenceUsers = await GetLicenceUserAssignmentsAsync(licence.Id);

            var model = new LicenceEditViewModel
            {
                Id = licence.Id,
                LicenceName = licence.LicenceName,
                LicenceActiveDate = licence.LicenceActiveDate,
                LicenceEndDate = licence.LicenceEndDate,
                LicenceUser = licenceUsers,
                JoyStaffs = joyStaff
            };
            return model;
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
        public async Task<bool> AddLicence(LicenceEditViewModel model)
        {

            var licence = new Licence
            {
                LicenceName = model.LicenceName,
                LicenceActiveDate = model.LicenceActiveDate,
                LicenceEndDate = model.LicenceEndDate
            };
            _context.Licence.Add(licence);
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
        public async Task<LicenceUser> GetAssignmentByIdAsync(int licenceAssigmentId)
        {
            var licenceAssigment = await _context.LicenceUser
                .Where(a => a.Id == licenceAssigmentId)
                .FirstOrDefaultAsync();
            return licenceAssigment;
        }
        public async Task<bool> DeleteLicenceAssigmentAsync(int licenceAssignmentId)
        {

            var assignment = await GetAssignmentByIdAsync(licenceAssignmentId);
            if (assignment != null)
            {
                _context.LicenceUser.Remove(assignment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AssignLicenceToStaffAsync(LicenceEditViewModel model)
        {
            var licence = await _context.Licence.FindAsync(model.Id);
            var joyStaff = await _staffManager.GetAllStaffsAsync();

            if (licence == null)
            {
                return false; 
            }

            model.JoyStaffs = joyStaff;

            if (model.SelectedStaffId.HasValue)
            {
                var selectedStaff = joyStaff.FirstOrDefault(staff => staff.Id == model.SelectedStaffId.Value);

                if (selectedStaff != null)
                {
                    var assignment = new LicenceUser
                    {
                        LicenceId = licence.Id,
                        StaffId = selectedStaff.Id,
                    };

                    _context.LicenceUser.Add(assignment);
                    await _context.SaveChangesAsync();

                    return true; 
                }
            }

            return false; // Personel seçilmemişse veya personel bulunamamışsa false dönecek
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

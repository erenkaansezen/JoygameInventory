using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace JoygameInventory.Business.Services
{

    public class JoyStaffService : IJoyStaffService
    {
        private readonly InventoryContext _context;
        private readonly IAssigmentService _assignmentService;
        private readonly ITeamService _teamService;
        private readonly ILicenceService _licenceService;
        public JoyStaffService(InventoryContext context, IAssigmentService assignmentService, ITeamService teamService, ILicenceService licenceService)
        {
            _context = context;
            _assignmentService = assignmentService;
            _teamService = teamService;
            _licenceService = licenceService;
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

        public async Task<IEnumerable<JoyStaff>> GetStaffListAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return await GetAllStaffsAsync();
            }
            else
            {
                return await SearchStaff(searchTerm);
            }

        }

        public async Task<StaffEditViewModel> GetStaffDetailsAsync(int id)
        {
            var staff = await GetStaffByIdAsync(id);
            if (staff != null)
            {
                var inventoryAssignments = await _assignmentService.GetUserAssignmentsAsync(staff.Id);
                var userTeams = await _teamService.GetUserAssignmentsAsync(staff.Id);
                var userLicences = await _licenceService.GetUserLicenceAssignmentsAsync(staff.Id);
                var teams = await _teamService.GetAllTeamsAsync();

                var model = new StaffEditViewModel
                {
                    Id = staff.Id,
                    Name = staff.Name,
                    Surname = staff.Surname,
                    Email = staff.Email,
                    PhoneNumber = staff.PhoneNumber,
                    Document = staff.Document,
                    InventoryAssigments = inventoryAssignments,
                    UserTeam = userTeams,
                    Team = teams,
                    LicencesUser = userLicences
                };

                return model;
            }
            return null;


        }
    }
}


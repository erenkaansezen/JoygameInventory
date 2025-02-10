using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class TeamService : ITeamService
    {
        private readonly InventoryContext _context;

        // Constructor ile context'i alıyoruz
        public TeamService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<UserTeam>> GetUserAssignmentsAsync(int userId)
        {
            var usersTeam = await _context.userTeam
                .Include(ia => ia.Team)
                .Include(ia => ia.Staff)  // Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.StaffId == userId)  // Kullanıcıya ait zimmetli envanterleri al
                .ToListAsync();

            return usersTeam;
        }
        public async Task<List<UserTeam>> GetTeamUserAssignmentsAsync(int userId)
        {
            var usersTeam = await _context.userTeam
                .Include(ia => ia.Team)
                .Include(ia => ia.Staff)  // Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.TeamId == userId)  // Kullanıcıya ait zimmetli envanterleri al
                .ToListAsync();

            return usersTeam;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            // Veritabanından tüm staff'leri asenkron şekilde çekiyoruz
            return await _context.Teams.ToListAsync();
        }
        public async Task<UserTeam> GetUserTeamByIdAsync(int id)
        {
            // Veritabanından staff'ı ID ile arıyoruz
            return await _context.userTeam.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Team> GetTeamByIdAsync(int id)
        {
            // Veritabanından staff'ı ID ile arıyoruz
            return await _context.Teams.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<bool> IsTeamUnique(string teamName)
        {
            return !await _context.Teams.AnyAsync(s => s.TeamName == teamName);
        }
        public async Task<bool> AddTeam(Team Team)
        {
            _context.Teams.Add(Team);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task UpdateTeamAsync(UserTeam userteams)
        {
            _context.userTeam.Update(userteams);
            await _context.SaveChangesAsync();
        }
        public async Task AddUserTeam(UserTeam staffTeam)
        {
            _context.userTeam.Add(staffTeam);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Team>> SearchTeam(string searchTerm)
        {
            var query = _context.Teams.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(server => EF.Functions.Like(server.TeamName, "%" + searchTerm + "%"));
            }

            return await query.ToListAsync();
        }
        public async Task DeleteTeamAsync(int id)
        {
            var deleteTeam = await _context.Teams.FindAsync(id);
            if (deleteTeam != null)
            {
                _context.Teams.Remove(deleteTeam);
                await _context.SaveChangesAsync();
            }
        }
    }
}

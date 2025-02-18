using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class TeamService : ITeamService
    {
        private readonly InventoryContext _context;

        public TeamService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<List<UserTeam>> GetUserAssignmentsAsync(int userId)
        {
            var usersTeam = await _context.userTeam
                .Include(ia => ia.Team)
                .Include(ia => ia.Staff)  
                .Where(ia => ia.StaffId == userId)  
                .ToListAsync();

            return usersTeam;
        }
        public async Task<List<UserTeam>> GetTeamUserAssignmentsAsync(int userId)
        {
            var usersTeam = await _context.userTeam
                .Include(ia => ia.Team)
                .Include(ia => ia.Staff)  
                .Where(ia => ia.TeamId == userId)  
                .ToListAsync();

            return usersTeam;
        }

        public async Task<List<Team>> GetAllTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }
        public async Task<UserTeam> GetUserTeamByIdAsync(int id)
        {
            return await _context.userTeam.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Team> GetTeamByIdAsync(int id)
        {
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
        public async Task TeamUpdateAsync(Team team)
        {
            _context.Teams.Update(team);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTeamAsync(UserTeam userTeams)// staffdetails üzerinden çalışır
        {
            _context.userTeam.Update(userTeams);
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

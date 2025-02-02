using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class TeamService
    {
        private readonly InventoryContext _context;

        // Constructor ile context'i alıyoruz
        public TeamService(InventoryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserTeam>> GetUserAssignmentsAsync(int userId)
        {
            var usersTeam = await _context.userTeam
                .Include(ia => ia.Team)
                .Include(ia => ia.Staff)  // Envanterin ait olduğu ürünü de dahil et
                .Where(ia => ia.StaffId == userId)  // Kullanıcıya ait zimmetli envanterleri al
                .ToListAsync();

            return usersTeam;
        }


        public async Task<List<Team>> GetAllTeamsAsync()
        {
            // Veritabanından tüm staff'leri asenkron şekilde çekiyoruz
            return await _context.Teams.ToListAsync();
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
    }
}

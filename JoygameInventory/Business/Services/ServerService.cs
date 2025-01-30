using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace JoygameInventory.Business.Services
{
    public class ServerService
    {
        private readonly InventoryContext _context;

        public ServerService(InventoryContext context)
        {
            _context = context;
        }
        public async Task<List<Servers>> GetAllServersAsync()
        {
            // Veritabanından tüm staff'leri asenkron şekilde çekiyoruz
            return await _context.Servers.ToListAsync();
        }

        public async Task<IEnumerable<Servers>> SearchStaff(string searchTerm)
        {
            var query = _context.Servers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(server => EF.Functions.Like(server.ServerName, "%" + searchTerm + "%"));
            }

            return await query.ToListAsync();
        }

        public async Task<Servers> GetServerByIdAsync(int id)
        {
            return await _context.Servers.FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task UpdateServerAsync(Servers server)
        {
            _context.Servers.Update(server);
             await _context.SaveChangesAsync();

        }

        public async Task<bool> CreateServer(Servers servers)
        {
            _context.Servers.Add(servers);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}

using JoygameInventory.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JoygameInventory.Business.Services
{
    public interface ITeamService
    {
        // Kullanıcıya ait takım zimmetlerini al
        Task<List<UserTeam>> GetUserAssignmentsAsync(int userId);

        // Takıma ait kullanıcı zimmetlerini al
        Task<List<UserTeam>> GetTeamUserAssignmentsAsync(int userId);

        // Tüm takımları al
        Task<List<Team>> GetAllTeamsAsync();

        // Kullanıcı takımını ID ile al
        Task<UserTeam> GetUserTeamByIdAsync(int id);

        // Takımı ID ile al
        Task<Team> GetTeamByIdAsync(int id);

        // Takım adının benzersiz olup olmadığını kontrol et
        Task<bool> IsTeamUnique(string teamName);

        // Yeni bir takım ekle
        Task<bool> AddTeam(Team Team);

        Task TeamUpdateAsync(Team team);


        // Kullanıcı takımını güncelle
        Task UpdateTeamAsync(UserTeam userteams);

        // Yeni bir kullanıcı takım ilişkisi ekle
        Task AddUserTeam(UserTeam staffTeam);

        // Takım araması yap
        Task<IEnumerable<Team>> SearchTeam(string searchTerm);

        // Takımı sil
        Task DeleteTeamAsync(int id);
    }
}

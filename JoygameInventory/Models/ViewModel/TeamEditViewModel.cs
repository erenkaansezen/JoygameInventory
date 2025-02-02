using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class TeamEditViewModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }

        public List<UserTeam> Teams { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Data.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        public string TeamName { get; set; } 

        public List<UserTeam> Teams { get; set; }

    }
}

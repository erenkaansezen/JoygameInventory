using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Data.Entities
{
    public class UserTeam
    {
        [Key]   
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int TeamId { get; set; }

        public JoyStaff Staff { get; set; }
        public Team Team { get; set; }


    }
}

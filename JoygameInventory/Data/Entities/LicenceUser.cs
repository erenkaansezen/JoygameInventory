using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Data.Entities
{
    public class LicenceUser
    {
        [Key]
        public int Id { get; set; }
        public int LicenceId { get; set; }
        public int StaffId { get; set; }

        public Licence Licence { get; set; }
        public JoyStaff staff { get; set; }
    }
}

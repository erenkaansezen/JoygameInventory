using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Data.Entities
{
    public class JoyStaff 
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Soyisim alanı zorunludur.")]

        public string Surname { get; set; } = null!;

        [Required(ErrorMessage = "Email alanı zorunludur.")]

        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }

        public string? Document { get; set; }

        public ICollection<UserTeam> Teams { get; set; }

        public ICollection<InventoryAssigment>? InventoryAssigments { get; set; }
        public ICollection<AssigmentHistory>? AssigmentHistorys { get; set; }

    }
}

using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class StaffEditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "İsim alanı zorunludur.")]

        public string? Name { get; set; }

        [Display(Name = "Kullanıcı Soyadı")]
        [Required(ErrorMessage = "Soyisim alanı zorunludur.")]

        public string? Surname { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email alanı zorunludur.")]

        [Display(Name = "Email Adresi")]
        public string? Email { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }


        public string? Document { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir takım seçin.")]
        public int SelectedTeamId { get; set; }


        public IEnumerable<Team>? Team { get; set; }
        public IEnumerable<InventoryAssigment>? InventoryAssigments { get; set; }
        public IEnumerable<UserTeam>? UserTeam { get; set; }



    }




}

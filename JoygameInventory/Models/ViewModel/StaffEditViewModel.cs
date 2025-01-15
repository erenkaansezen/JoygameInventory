using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class StaffEditViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string? Name { get; set; }

        [Display(Name = "Kullanıcı Surname")]
        public string? Surname { get; set; }

        [EmailAddress]
        [Display(Name = "Email Adresi")]
        public string? Email { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;


        public string? Document { get; set; } = string.Empty;


        public IEnumerable<InventoryAssigment> InventoryAssigments { get; set; } 


    }




}

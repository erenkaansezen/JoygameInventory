using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class UserEditViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string? FullName { get; set; }

        [EmailAddress]
        [Display(Name = "Email Adresi")]
        public string? Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "Kullanıcı Şifresi")]
        public string? Password { get; set; } = string.Empty;

        [DataType(DataType.Password)] // Data tipi belirtilir
        [Compare(nameof(Password), ErrorMessage = "Parola Eşleşmiyor")] // Hata mesajı çıkarır
        [Display(Name = "Şifrenizi Doğrulayınız")]
        public string? ConfirmPassword { get; set; } = string.Empty;

        public IList<string>? SelectedRoles { get; set; }

        // Zimmetli envanterler
        public IEnumerable<InventoryAssigment> InventoryAssigments { get; set; } // Koleksiyon
    }




}

using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class LoginViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string? Email { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Şifre gereklidir.")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; } = true;
    }
}

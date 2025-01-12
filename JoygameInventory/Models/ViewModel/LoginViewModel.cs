using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class LoginViewModel
    {
        public int UserId { get; set; }

        [EmailAddress]
        public string? Email { get; set; } = null!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; } = true;
    }
}

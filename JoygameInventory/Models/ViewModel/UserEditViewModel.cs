using JoygameInventory.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace JoygameInventory.Models.ViewModel
{
    public class UserEditViewModel
    {

            public string Id { get; set; }

            [Display(Name = "Kullanıcı adı")]
            [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
            public string UserName { get; set; }

            [Display(Name = "Kullanıcı Email")]
            [Required(ErrorMessage = "Email gereklidir.")]
            [EmailAddress(ErrorMessage = "Geçersiz email adresi.")]
            public string Email { get; set; }

            [Display(Name = "Kullanıcı Telefon Numarası")]
            [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
            public string PhoneNumber { get; set; }


            [Display(Name = "Kullanıcı Şifresi")]
            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Geçerli bir telefon numarası girin.")]

            public string Password { get; set; }

            [Display(Name = "Şifreyi Doğrulayın")]
            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Parolalar eşleşmiyor.")]
            public string ConfirmPassword { get; set; }
        


    }




}

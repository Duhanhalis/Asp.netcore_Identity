using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityCoreApp.Web.SignUpModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
            
        }
        public SignInViewModel(string email,string password)
        {
            Email= email;
            Password= password;
        }
        [Required(ErrorMessage = "Email Bos Birakilamaz")]
        [EmailAddress(ErrorMessage ="Emaiol Formati Eksik")]
        [Display(Name = "Email :")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Sifre Bos Birakilamaz")]
        [Display(Name = "Sifre :")]
        public string? Password { get; set; }

        [Display(Name = "Beni Hatirla")]
        public bool RememberMe { get; set; }
    }
}

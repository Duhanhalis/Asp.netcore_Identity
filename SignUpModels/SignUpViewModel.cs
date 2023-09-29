using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityCoreApp.Web.SignUpModels
{
    public class SignUpViewModel
    {
        public SignUpViewModel() { }
        public SignUpViewModel(string userName, string email, string phone, string password)
        {
            UserName = userName;
            Email = email;
            Phone = phone;
            Password = password;
        }
        [Required(ErrorMessage ="Kullanici Adi Bos Birakilamaz")]
        [Display(Name = "Kullanici Adi :")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage ="Email Formati Yanlistir")]
        [Required(ErrorMessage = "Email Alani Bos Birakilamaz")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon Numarasi Bos Birakilamaz")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Sifre Adi Bos Birakilamaz")]
        [Display(Name = "Sifre :")]
        [MinLength(6, ErrorMessage = "Sifreniz En Az 6 Karekter Olabilir")]
        public string Password { get; set; }

        [Compare(nameof(Password),ErrorMessage = "Girmis Oldugunuz Sifreler Eslesmiyor")]
        [Required(ErrorMessage = "Sifre Kontrol Bos Birakilamaz")]
        [Display(Name = "Sifre Tekrar :")]
        public string PasswordConfirm { get; set; }


    }
}

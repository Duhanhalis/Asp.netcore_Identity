using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityCoreApp.Web.Models
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Kullanici Adi Bos Birakilamaz")]
        [Display(Name = "Kullanici Adi :")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email Formati Yanlistir")]
        [Required(ErrorMessage = "Email Alani Bos Birakilamaz")]
        [Display(Name = "Email :")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon Numarasi Bos Birakilamaz")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Dogum Tarihi Bos Birakilamaz")]
        [Display(Name = "Dogum Tarihi :")]
        public DateTime? BirthDate { get; set; }

        [Required(ErrorMessage = "Sehir Bos Birakilamaz")]
        [Display(Name = "Sehir :")]
        public string City { get; set; }

        [Display(Name = "Profil Resmi :")]
        public IFormFile? Picture { get; set; }

        [Required(ErrorMessage = "Cinsiyet Kismi Bos Birakilamaz")]
        [Display(Name = "Cinsiyet :")]
        public Gender? Cinsiyet { get; set; }

    }
}

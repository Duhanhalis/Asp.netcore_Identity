using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityCoreApp.Web.Models
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Sifre Alani Bos Birakilamaz")]
        [Display(Name = "Eski Sifre :")]
        [MinLength(6, ErrorMessage = "Sifreniz En Az 6 Karekter Olabilir")]
        public string PasswordOld { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Yeni Sifre Alani Bos Birakilamaz")]
        [Display(Name = "Yeni Sifre :")]
        [MinLength(6, ErrorMessage = "Sifreniz En Az 6 Karekter Olabilir")]
        public string PasswordNew { get; set; } = null!;

        [Compare(nameof(PasswordNew),ErrorMessage ="Sifreler Ayni Degil")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Sifreniz En Az 6 Karekter Olabilir")] 
        [Required(ErrorMessage = "Yeni Sifre Tekrar Alani Bos Birakilamaz")]
        [Display(Name = "Yeni Sifre Onay :")]
        public string PasswordConfirm { get; set; } = null!;
    }
}

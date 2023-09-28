using System.ComponentModel.DataAnnotations;

namespace AspNetIdentityCoreApp.Web.Models
{
    public class ForgetPasswordViewModel
    {
        [
            EmailAddress(ErrorMessage = "Emaiol Formati Eksik"),
            Required(ErrorMessage = "Email Bos Birakilamaz"),
            Display(Name = "Email :")
        ]
        public string? Email { get; set; }
    }
}

using AspNetIdentityCoreApp.Web.OptionsModels;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AspNetIdentityCoreApp.Web.Services
{
    public class EmailService:IEmailService
    {
        public readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendResetPasswordEmail(string resetEmailLink, string To)
        {
            var smptClient = new SmtpClient();
            smptClient.Host = _emailSettings.Host;
            smptClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smptClient.UseDefaultCredentials = false;
            smptClient.Port = 587;
            smptClient.Credentials = new NetworkCredential(_emailSettings.Email,_emailSettings.Password);
            smptClient.EnableSsl = true;
            var mail = new MailMessage();
            mail.From = new MailAddress(_emailSettings.Email);
            mail.To.Add(resetEmailLink);
            mail.Subject = "Sifre Sifirlama Linki";
            mail.Body = @$"
                <h4>Sifrenizi Yenilemek Icin Asagida ki Link e Tiklayin</h4>
                <hr>
                <p><a href='${resetEmailLink}'>Sifre Yenileme Link</p></a>" ;
            mail.IsBodyHtml= true;
            await smptClient.SendMailAsync(mail);
        }

    }
}

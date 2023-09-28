namespace AspNetIdentityCoreApp.Web.Services
{
    public interface IEmailService
    {
       public Task SendResetPasswordEmail(string resetEmailLink, string To);

    }
}

using Microsoft.AspNetCore.Identity;

namespace AspNetIdentityCoreApp.Web.Localizations
{
    public class LocalizationIdentityErrorDescriber:IdentityErrorDescriber
    {
        public override IdentityError InvalidEmail(string? email)
        {
            return new IdentityError { Code = "DuplicateEmail", Description = $"Bu {email} Alinmistir" };
            //return base.InvalidEmail(email);
        }
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Code = "DuplicateEmail", Description = $"Bu {email} Alinmistir" };
            //return base.DuplicateEmail(email);
        }
    }
}

using AspNetIdentityCoreApp.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace AspNetIdentityCoreApp.Web.CustomValidations
{
    public class PasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string? password)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (password!.ToLower().Contains(user.UserName!.ToLower()))
            {
                errors.Add(new IdentityError() { Code = "PasswordContainsUserName", Description = "Sifre Alani Kullanici Adi Iceremez" });
            }
            if (password!.ToLower().StartsWith("123456"))
            {
                errors.Add(new IdentityError() { Code = "PasswordContains123456", Description = "Sifre Alani Basit Olmamali" });
            }
            if (errors.Any())
            {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
            return Task.FromResult(IdentityResult.Success);
                
            
        }
    }
}

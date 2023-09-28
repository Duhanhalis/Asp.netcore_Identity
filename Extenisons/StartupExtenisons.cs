using AspNetIdentityCoreApp.Web.CustomValidations;
using AspNetIdentityCoreApp.Web.Localizations;
using AspNetIdentityCoreApp.Web.Models;
using AspNetIdentityCoreApp.Web.Services;
using Microsoft.AspNetCore.Identity;

namespace AspNetIdentityCoreApp.Web.Extenisons
{
    public static class StartupExtenisons
    {
        public static void AddIdentityWithExt(this IServiceCollection services)
        {
            services.Configure<DataProtectionTokenProviderOptions>(options =>
            options.TokenLifespan = TimeSpan.FromMinutes(120)
            );
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "abcçdefghıijklmnopqrsştuüvwxyzABCÇDEFGHIİJKLMNOPQRSŞTUÜVWXYZ0123456789-._@+";


                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                options.Lockout.MaxFailedAccessAttempts = 3;
            }).AddPasswordValidator<PasswordValidator>().AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddUserValidator<UserValidator>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }
    }
    //
}

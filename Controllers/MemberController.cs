using AspNetIdentityCoreApp.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetIdentityCoreApp.Web.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public MemberController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var userViewModel = new UserViewModel { Email = currentUser!.Email,PhoneNumber=currentUser.PhoneNumber,UserName=currentUser.UserName };
            return View(userViewModel);
        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }
        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PasswordChangeAsync(PasswordChangeViewModel request)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name!);
            var checkOldPassword = await _userManager.CheckPasswordAsync(currentUser, request.PasswordOld);

            if(!checkOldPassword)
            {
                ModelState.AddModelError(string.Empty, "Eski Sifreniz Yanlis");
                return View();
            }
            var resultChangePassword = await _userManager.ChangePasswordAsync(currentUser,request.PasswordOld, request.PasswordNew);

            if(!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x=>x.Description).ToList());
            }
            await _userManager.UpdateSecurityStampAsync(currentUser);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(currentUser, request.PasswordNew, true, false);
            TempData["SuccesMessage"] = "Sifreniz Basariyla degistirlmistir.";

            return View();
        }
    }
}

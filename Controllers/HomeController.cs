using AspNetIdentityCoreApp.Web.Models;
using AspNetIdentityCoreApp.Web.Services;
using AspNetIdentityCoreApp.Web.SignUpModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace AspNetIdentityCoreApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> _userManager, SignInManager<AppUser> SignInManager, IEmailService emailService)
        {
            _UserManager = _userManager;
            _logger = logger;
            _signInManager = SignInManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel model, string? returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Action("Index", "Home");

            var hasUser = await _UserManager.FindByEmailAsync(model.Email);

            if (hasUser == null)
            {
                ModelState.AddModelError(string.Empty, "Email veya Sifre Yanlis");
                return View();
            }
            

           
            var signInResult = await _signInManager.PasswordSignInAsync(hasUser, model.Password, model.RememberMe, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 Defa Yanlis Girdiniz 3 Dk Giremeyeceksiniz." });
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelErrorList(new List<string>() { $"Email veya Sifre Yanlis", $" (Basarisiz Giris Sayisi : {await _UserManager.GetAccessFailedCountAsync(hasUser)})" });
                return View();
            }
            if (hasUser.BirthDate.HasValue)
            {
                await _signInManager.SignInWithClaimsAsync(hasUser, model.RememberMe, new[] { new Claim("birthdate", hasUser.BirthDate.Value.ToString()) });
            }
            return Redirect(returnUrl);
            
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var identityResult = await _UserManager.CreateAsync(new AppUser() { UserName = request.UserName, PhoneNumber = request.Phone, Email = request.Email }, request.Password);
            if (!identityResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }

            var exchangeExpireClaim = new Claim("ExchangeExpireDate", DateTime.Now.AddDays(10).ToString());
            var user = await _UserManager.FindByNameAsync(request.UserName);
            var claimResult = await _UserManager.AddClaimAsync(user, exchangeExpireClaim);
            if (!claimResult.Succeeded)
            {
                ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
                return View();
            }
            TempData["SuccesMessage"] = "Uyelik Kayit Islemi Basarila Gerceklestirmistir.";

            return View(nameof(HomeController.SignUp));

        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            var hasUser = await _UserManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelError(String.Empty, "Bu Email e Sahip Kullanici Bulunamamistir");
                return View();
            }
            string passwordResetToken = await _UserManager.GeneratePasswordResetTokenAsync(hasUser);
            var passwordRestLink = Url.Action("ResetPassword", "Home", new { userId = hasUser.Id, Token = passwordResetToken },
                HttpContext.Request.Scheme, "");

            await _emailService.SendResetPasswordEmail(passwordRestLink, hasUser.Email);

            TempData["Success"] = "Email Sifirlama Mail i Gonderilmistir";
            return RedirectToAction(nameof(ForgetPassword));
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
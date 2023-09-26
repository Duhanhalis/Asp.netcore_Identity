using AspNetIdentityCoreApp.Web.Models;
using AspNetIdentityCoreApp.Web.SignUpModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetIdentityCoreApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> _userManager, SignInManager<AppUser> SignInManager)
        {
            _UserManager = _userManager;
            _logger = logger;
            _signInManager = SignInManager;
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

            if(signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelErrorList(new List<string>() { "3 Defa Yanlis Girdiniz 3 Dk Giremeyeceksiniz." });
                return View();
            }
            ModelState.AddModelErrorList(new List<string>() { $"Email veya Sifre Yanlis",$" (Basarisiz Giris Sayisi : {await _UserManager.GetAccessFailedCountAsync(hasUser)})" });
            
            return View();  
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

            if (identityResult.Succeeded)
            {
                TempData["SuccesMessage"] = "Uyelik Kayit Islemi Basarila Gerceklestirmistir.";
                return View(nameof(HomeController.SignUp));
            }
            ModelState.AddModelErrorList(identityResult.Errors.Select(x => x.Description).ToList());
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
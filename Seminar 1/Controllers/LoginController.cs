using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Seminar_1.Models.VMs;

namespace Seminar_1.Controllers
{
    public class LoginController : Controller
    {
        private readonly Seminar1Context context;

        public LoginController( Seminar1Context context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new LoginVM());
      
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "There were some errors in your form");
                return View(loginVM);
            }

            // var user = Models.Entities.User.GetAll().FirstOrDefault(u => u.UserName == loginVM.UserName);

                     
            var user = context.Users.FirstOrDefault(u => u.UserName ==loginVM.UserName);                
                
                    

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Credentials");
                return View(loginVM);
            }

            if (user.Password != Base64.Base64Encode(loginVM.Password))
            {
                ModelState.AddModelError(string.Empty, "Invalid Credentials");
                return View(loginVM);
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, loginVM.UserName),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true,
                IsPersistent = false,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

    
}
}

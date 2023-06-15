using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using newproject.Models;
using System.Security.Claims;
using TP1.Models;
using Microsoft.AspNetCore.Authorization;
namespace newproject.Controllers
{
  
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine("**** log out index ****");
            ClaimsPrincipal claimUser = HttpContext.User;

            if (claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            Console.WriteLine("**** log out index 2 ****");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("**** Model is valide  ****");
                var user = await _context.loginViewModels.FindAsync(model.Username);
                if (user != null && user.Password == model.Password && user.Username == model.Username)
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Username.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                     CookieAuthenticationDefaults.AuthenticationScheme);

                    AuthenticationProperties properties = new AuthenticationProperties()
                    {

                        AllowRefresh = true,
                        IsPersistent = model.RememberMe
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), properties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }

           return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            Console.WriteLine("Logout successful.   1");
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Console.WriteLine("Logout successful.   2");
            return RedirectToAction("Index", "Login");
        }

    }
}
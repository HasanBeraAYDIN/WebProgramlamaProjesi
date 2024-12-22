using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wep_programlama_proje.Data;
using Microsoft.EntityFrameworkCore;

namespace wep_programlama_proje.Controllers
{
    public class LoginController : Controller
    {
        private DataContext _dataContext;

        public LoginController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]

        public async Task<IActionResult> Login(Data.Musteri model)
        {

            var user = await _dataContext.Musteriler.FirstOrDefaultAsync(M => M.MusteriEmail == model.MusteriEmail);

            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError(string.Empty, "geçersiz email veye şifre girdiniz");
                return View(model);
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.MusteriEmail),
                new Claim(ClaimTypes.NameIdentifier,user.MusteiId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");

        }

    }
}

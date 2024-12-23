using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using web_programlama_proje.Models;

namespace web_programlama_proje.Controllers
{
    public class KullaniciController : Controller
    {
        private DataContext _dataContext;
        public KullaniciController(DataContext context)
        {
            _dataContext = context;
        }
        
        public IActionResult Profil()
        {
            var kullaniciEmail = HttpContext.Session.GetString("KullaniciEmail");

            if (string.IsNullOrEmpty(kullaniciEmail))
            {
                // Eğer kullanıcı giriş yapmamışsa, login sayfasına yönlendir
                return RedirectToAction("Login", "Kullanici");
            }

            // Kullanıcı giriş yaptıysa, profil bilgilerini getirebilirsiniz
            var user = _dataContext.Kullanicilar.FirstOrDefault(k => k.KullaniciEmail == kullaniciEmail);

            if (user == null)
            {
                return RedirectToAction("Login", "Kullanici");
            }

            return View(); // Profil sayfasını kullanıcının bilgileriyle döndür
        }
        
        public IActionResult Login()
        {
            return View();
        }
        
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(string KullaniciEmail,string Password)
        {

            var user = await _dataContext.Kullanicilar
       .FirstOrDefaultAsync(k => k.KullaniciEmail == KullaniciEmail);

            if (user == null)
            {
                Console.WriteLine("Kullanıcı bulunamadı.");
                ModelState.AddModelError("", "Kullanılan Email Geçersiz");
                return View();
            }

            if (Password == user.Password)
            {
                
                Console.WriteLine($"Giriş başarılı: {user.KullaniciEmail}");
                HttpContext.Session.SetString("KullaniciAd", user.KullaniciAd);
                HttpContext.Session.SetString("KullaniciEmail", user.KullaniciEmail);
                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine("Şifre hatalı.");
            ModelState.AddModelError("Password", "Şifre yanlış");
            return View();

        }

        [HttpPost]

        public async Task<IActionResult> Register(Models.Kullanici model)
        {
            if (!ModelState.IsValid)
            {
                var kullanici = await _dataContext.Kullanicilar
                    .FirstOrDefaultAsync(k => k.KullaniciEmail == model.KullaniciEmail);

                if (kullanici != null)
                {
                    ModelState.AddModelError("KullaniciEmail", "Bu email zaten kayıtlı.");
                    return View(model);
                }


                _dataContext.Kullanicilar.Add(model);
                await _dataContext.SaveChangesAsync();

                return RedirectToAction("Login", "Kullanici");
            }



            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            
            HttpContext.Session.Clear();

            
            return RedirectToAction("Index", "Home");
        }

    }
}

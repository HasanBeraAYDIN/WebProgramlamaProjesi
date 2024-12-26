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
        public IActionResult Duzenle()
        {
            return View();
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
                Console.WriteLine($"Kullanıcı ID: {user.KullaniciId}"); // Debug için
                Console.WriteLine($"Giriş başarılı: {user.KullaniciEmail}");
                HttpContext.Session.SetInt32("KullaniciId", user.KullaniciId);
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

        [HttpPost]

        public async Task<IActionResult> Duzenle(string YeniAd, string YeniEmail)
        {
            // Session'dan giriş yapmış kullanıcının Email veya ID bilgisini alıyoruz.
            var sessionEmail = HttpContext.Session.GetString("KullaniciEmail");
            if (string.IsNullOrEmpty(sessionEmail))
            {
                Console.WriteLine("kullanıcı email boş");
                return RedirectToAction("Login", "Kullanici"); // Oturum yoksa login sayfasına yönlendir.
            }
            Console.WriteLine("ilk if koşulunu geçti");
            // Veritabanından kullanıcıyı bul.
            var user = await _dataContext.Kullanicilar.FirstOrDefaultAsync(u => u.KullaniciEmail == sessionEmail);
            if (user == null)
            {
                Console.WriteLine("kullanıcı bulunamadı");
                return NotFound("Kullanıcı bulunamadı."); // Kullanıcı bulunmazsa hata döndür.
            }
            Console.WriteLine("ikinci if koşulunu geçti");
            // Formdan gelen değerlerle kullanıcı bilgilerini güncelle.
            user.KullaniciAd = YeniAd;
            user.KullaniciEmail = YeniEmail;
            user.Password = user.Password;
            Console.WriteLine("kullanıcı bilgileri güncellendi");
            Console.WriteLine($"YENİ AD: {user.KullaniciAd}");
            Console.WriteLine($"YENİ EMAİL: {user.KullaniciEmail}");

            // Veritabanında değişiklikleri kaydet.
            _dataContext.Kullanicilar.Update(user);
            await _dataContext.SaveChangesAsync();

            Console.WriteLine("değiliklikler kaydedildi");

            // Session'daki bilgileri güncelle.
            HttpContext.Session.SetString("KullaniciAd", user.KullaniciAd);
            HttpContext.Session.SetString("KullaniciEmail", user.KullaniciEmail);

            return RedirectToAction("Profil", "Kullanici"); // Profil sayfasına yönlendir.
        }


    }
}

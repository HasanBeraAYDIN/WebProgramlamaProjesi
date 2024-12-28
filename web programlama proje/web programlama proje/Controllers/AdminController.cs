using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_programlama_proje.Models;

namespace web_programlama_proje.Controllers
{
   // [Route("api/[controller]")]
    //[ApiController]
    public class AdminController : Controller
    {
        private DataContext _dataContext;
        public AdminController(DataContext context)
        {
            _dataContext = context;
        }      

        public IActionResult Calisanlar()
        {
            return View(); 
        }
     
        public IActionResult Admin()
        {
            return View();
        }

    public IActionResult AdminPanel()
    {
            // Session'dan AdminId bilgisini al
            var adminId = HttpContext.Session.GetInt32("AdminId");

            // Eğer AdminId null ise, kullanıcı giriş yapmamış demektir.
            if (adminId == null)
            {
                return RedirectToAction("Admin", "Admin");
            }

            // AdminId'ye göre doğrulama yap
            var admin = _dataContext.Adminler.FirstOrDefault(k => k.AdminId == adminId);

            // Eğer admin bilgisi bulunamazsa, login sayfasına yönlendir
            if (admin == null)
            {
                return RedirectToAction("Admin", "Admin");
            }

            return View();
    }

    [HttpPost]
    public async Task<IActionResult> Admin(string AdminAd, string AdminPassword)
    {
        // Session'ı temizle
        HttpContext.Session.Clear();

        // Veritabanında AdminAd ile eşleşen kullanıcıyı asenkron şekilde al
        var admin = await _dataContext.Adminler
            .FirstOrDefaultAsync(k => k.AdminAd == AdminAd);

        // Eğer kullanıcı bulunamazsa
        if (admin == null)
        {
            Console.WriteLine("Admin bulunamadı.");
            ModelState.AddModelError("AdminAd", "Kullanıcı bulunamadı.");
            return View();
        }

        // Şifre doğrulama
        if (AdminPassword == admin.AdminPassword)
        {
            // Kullanıcı ID'sini Session'a kaydet
            HttpContext.Session.SetInt32("AdminId", admin.AdminId);
            return RedirectToAction("AdminPanel", "Admin");
        }

        // Şifre yanlışsa hata mesajı göster
        Console.WriteLine("Şifre hatalı.");
        ModelState.AddModelError("Password", "Şifre yanlış.");
        return View();
    }

    [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();


            return RedirectToAction("Index", "Home");
        }

    }
}

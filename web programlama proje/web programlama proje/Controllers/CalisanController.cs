using Microsoft.AspNetCore.Mvc;
using web_programlama_proje.Models;

namespace web_programlama_proje.Controllers
{
    public class CalisanController : Controller
    {
        private readonly DataContext _dataContext;
        public CalisanController(DataContext dataContext) 
        { 
            _dataContext = dataContext;
        }
        public IActionResult Calisanlar()
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

            var calisanlar = _dataContext.Calisanlar.ToList(); // _context doğru şekilde yapılandırılmış olmalı
            return View(calisanlar);
        }
        public IActionResult CalisanEkle()
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
        public IActionResult CalisanEkle(Calisan calisan) 
        {
            if (ModelState.IsValid)
            {
                _dataContext.Calisanlar.Add(calisan);
                _dataContext.SaveChanges();
                return RedirectToAction("Calisanlar","Calisan");
            }
            return RedirectToAction("CalianEkle","Calisan");
        }
        [HttpGet]
        public IActionResult Sil(int id)
        {
            // Veritabanından çalışanı bul
            var calisan = _dataContext.Calisanlar.FirstOrDefault(c => c.CalisanId == id);

            if (calisan != null)
            {
                // Çalışanı veritabanından kaldır
                _dataContext.Calisanlar.Remove(calisan);
                _dataContext.SaveChanges();
            }

            // Çalışan listesi sayfasına geri yönlendir
            return RedirectToAction("Calisanlar");
        }

    }
}


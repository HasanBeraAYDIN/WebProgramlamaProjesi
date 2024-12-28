using Microsoft.AspNetCore.Mvc;
using web_programlama_proje.Models;

namespace web_programlama_proje.Controllers
{
    public class HizmetController : Controller
    {
        private readonly DataContext _dataContext;

        public HizmetController(DataContext context)
        {
            _dataContext = context;
        }
        public IActionResult Hizmetler()
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
            var hizmetler = _dataContext.Hizmetler.ToList(); // Hizmetleri veritabanından al
            return View(hizmetler); // Hizmetleri view'a gönder

        }
        public IActionResult HizmetEkle()
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
        public IActionResult Ekle(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Hizmetler.Add(hizmet); // Hizmetler, DbContext'teki DbSet<Hizmet>'i temsil eder
                _dataContext.SaveChanges();
                return RedirectToAction("Hizmetler","Hizmet"); // Hizmet listeleme sayfasına yönlendir
            }
            return View(hizmet);
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            var hizmet = _dataContext.Hizmetler.FirstOrDefault(h => h.HizmetId == id);
            if (hizmet != null)
            {
                _dataContext.Hizmetler.Remove(hizmet);
                _dataContext.SaveChanges();
                return RedirectToAction("Hizmetler","Hizmet"); // Listeleme sayfasına yönlendirme
            }

            return NotFound();
        }
    }
}

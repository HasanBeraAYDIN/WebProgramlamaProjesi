using Microsoft.AspNetCore.Mvc;
using web_programlama_proje.Models;
using System.Linq;

public class CalisanHizmetController : Controller
{
    private readonly DataContext _context; // DbContext'inizi burada tanımlayın

    public CalisanHizmetController(DataContext context)
    {
        _context = context;
    }

    // Çalışan hizmet ekleme formu
    public IActionResult Ekle()
    {
        // Session'dan AdminId bilgisini al
        var adminId = HttpContext.Session.GetInt32("AdminId");

        // Eğer AdminId null ise, kullanıcı giriş yapmamış demektir.
        if (adminId == null)
        {
            return RedirectToAction("Admin", "Admin");
        }

        // AdminId'ye göre doğrulama yap
        var admin = _context.Adminler.FirstOrDefault(k => k.AdminId == adminId);

        // Eğer admin bilgisi bulunamazsa, login sayfasına yönlendir
        if (admin == null)
        {
            return RedirectToAction("Admin", "Admin");
        }
        ViewBag.Calisanlar = _context.Calisanlar.ToList(); // Tüm çalışanları al
        ViewBag.Hizmetler = _context.Hizmetler.ToList(); // Tüm hizmetleri al
        return View();
    }

    [HttpPost]
    public IActionResult Ekle(CalisanHizmet calisanHizmet)
    {
        Console.WriteLine("Ekleme işlemi başlıyor.");
        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState geçerli.");

            var calisan = _context.Calisanlar.Find(calisanHizmet.CalisanId);
            if (calisan == null)
            {
                Console.WriteLine($"Çalışan bulunamadı: CalisanId = {calisanHizmet.CalisanId}");
            }

            var hizmet = _context.Hizmetler.Find(calisanHizmet.HizmetId);
            if (hizmet == null)
            {
                Console.WriteLine($"Hizmet bulunamadı: HizmetId = {calisanHizmet.HizmetId}");
            }

            if (calisan != null && hizmet != null)
            {
                Console.WriteLine("Çalışan ve hizmet bulundu. Tabloya ekleniyor...");
                _context.calisanHizmet.Add(new CalisanHizmet
                {
                    CalisanId = calisanHizmet.CalisanId,
                    HizmetId = calisanHizmet.HizmetId
                });

                try
                {
                    _context.SaveChanges();
                    Console.WriteLine("Veritabanına başarıyla kaydedildi.");
                    return RedirectToAction("Calisanlar", "Calisan");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Veritabanına kaydedilirken hata oluştu: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine("ModelState geçersiz:");
            foreach (var modelState in ModelState)
            {
                Console.WriteLine($"Key: {modelState.Key}");
                foreach (var error in modelState.Value.Errors)
                {
                    Console.WriteLine($"Hata: {error.ErrorMessage}");
                }
            }
        }

        ViewBag.Calisanlar = _context.Calisanlar.ToList();
        ViewBag.Hizmetler = _context.Hizmetler.ToList();
        return View(calisanHizmet);
    }



}
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using web_programlama_proje.Models;
using Microsoft.EntityFrameworkCore;

public class RandevuController : Controller
{
    private readonly DataContext _context;

    public RandevuController(DataContext context)
    {
        _context = context;
    }
    public IActionResult AdminBekleyenRandevular()
    {
        // Tüm randevuları getir
        var tumRandevular = _context.Randevular.Include(r => r.Calisan)
                                               .Include(r => r.Hizmet)
                                               .Include(r => r.Kullanici);

        // Yalnızca "Pending" durumundaki randevuları filtrele
        var bekleyenRandevular = tumRandevular.Where(r => r.Durum == "Bekliyor").ToList();

        return View(bekleyenRandevular);
    }
    //public IActionResult Onayla() 
    //{
    //    return RedirectToAction("AdminBekleyenRandevular");
    //}

    // GET: Randevu/Create
    public IActionResult Create()
    {
        ViewBag.Calisanlar = new SelectList(_context.Calisanlar, "CalisanId", "CalisanAd");
        ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "HizmetId", "HizmetAd");
        return View();
    }

    [HttpGet]
    public JsonResult GetHizmetlerByCalisanId(int CalisanId)
    {
        // Çalışana atanmış hizmetleri alıyoruz
        var hizmetler = _context.Hizmetler
                                .Where(h => h.Calisanlar.Any(ch => ch.CalisanId == CalisanId))
                                .Select(h => new
                                {
                                    HizmetId = h.HizmetId,
                                    HizmetAd = h.HizmetAd
                                })
                                .ToList();

        return Json(hizmetler);
    }


    // POST: Randevu/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Randevu randevu)
    {
        Console.WriteLine(randevu.KullaniciId);
        randevu.Calisan = _context.Calisanlar.FirstOrDefault(c => c.CalisanId == randevu.CalisanId);
        randevu.Kullanici = _context.Kullanicilar.FirstOrDefault(k => k.KullaniciId == randevu.KullaniciId);
        randevu.Hizmet = _context.Hizmetler.FirstOrDefault(k => k.HizmetId == randevu.HizmetId);
        randevu.Durum = "Bekliyor";
        if (ModelState.IsValid)
        {
            return View(randevu);
        }
        // Çakışma kontrolü
        bool isConflict = _context.Randevular.Any(r =>
            r.CalisanId == randevu.CalisanId &&
            r.RandevuTarih == randevu.RandevuTarih
        // r.Durum == "Active" // Eğer aktif bir randevu varsa
        );

        if (isConflict)
        {
            ModelState.AddModelError("", "Bu tarih ve saatte seçilen çalışan için başka bir randevu mevcut.");
            return View(randevu);
        }
        _context.Randevular.Add(randevu);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home"); // Başarılı işlemden sonra yönlendirme

    }
    public async Task<IActionResult> KullaniciRandevular()
    {
        // Oturumdaki KullaniciId'yi al
        int kullaniciId = HttpContext.Session.GetInt32("KullaniciId") ?? 0;
        if (kullaniciId == 0)
        {
            return RedirectToAction("Login", "Kullanici"); // Kullanıcı giriş yapmamışsa Login sayfasına yönlendir
        }

        // Kullanıcı ID'sine göre randevuları al
        var randevular = await _context.Randevular
            .Include(r => r.Calisan)
            .Include(r => r.Hizmet)
            .Where(r => r.KullaniciId == kullaniciId)
            .OrderBy(r => r.RandevuTarih)
            .ToListAsync();

        // Eğer randevular bulunamazsa, konsola log yazdırabiliriz
        if (randevular == null || !randevular.Any())
        {
            Console.WriteLine("Bu kullanıcı için randevu bulunamadı.");
        }

        // Randevuları view'a gönder
        return View(randevular);
    }

}


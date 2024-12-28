using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using web_programlama_proje.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;

public class RandevuController : Controller
{
    private readonly DataContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public RandevuController(DataContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> AdminBekleyenRandevularAsync()
    {
        // "Bekliyor" durumundaki randevuları getir
        var bekleyenRandevular = await _context.Randevular
            .Include(r => r.Calisan)
            .Include(r => r.Hizmet)
            .Include(r => r.Kullanici)
            .Where(r => r.Durum == "Bekliyor")
            .ToListAsync();

        return View(bekleyenRandevular);
    }

    public IActionResult Create()
    {
        var kullaniciEmail = HttpContext.Session.GetString("KullaniciEmail");

        if (string.IsNullOrEmpty(kullaniciEmail))
        {
            // Eğer kullanıcı giriş yapmamışsa, login sayfasına yönlendir
            return RedirectToAction("Login", "Kullanici");
        }
        ViewBag.Calisanlar = new SelectList(_context.Calisanlar, "CalisanId", "CalisanAd");
        ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "HizmetId", "HizmetAd");
        return View();
    }

    [HttpGet]
    public JsonResult GetHizmetlerByCalisanId(int CalisanId)
    {
        var hizmetler = _context.Hizmetler
            .Where(h => h.Calisanlar.Any(ch => ch.CalisanId == CalisanId))
            .Select(h => new { h.HizmetId, h.HizmetAd })
            .ToList();

        return Json(hizmetler);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Randevu randevu)
    {
        var kullaniciEmail = HttpContext.Session.GetString("KullaniciEmail");

        if (string.IsNullOrEmpty(kullaniciEmail))
        {
            // Eğer kullanıcı giriş yapmamışsa, login sayfasına yönlendir
            return RedirectToAction("Login", "Kullanici");
        }

        if (ModelState.IsValid)
        {
            ViewBag.Calisanlar = new SelectList(_context.Calisanlar, "CalisanId", "CalisanAd");
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "HizmetId", "HizmetAd");
            return View(randevu);
        }

        // Çakışma kontrolü
        bool isConflict = _context.Randevular.Any(r =>
            r.CalisanId == randevu.CalisanId &&
            r.RandevuTarih == randevu.RandevuTarih &&
            r.Durum != "İptal Edildi" // İptal edilen randevular hariç
        );

        if (isConflict)
        {
            ModelState.AddModelError("", "Bu tarih ve saatte seçilen çalışan için başka bir randevu mevcut.");
            ViewBag.Calisanlar = new SelectList(_context.Calisanlar, "CalisanId", "CalisanAd");
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "HizmetId", "HizmetAd");
            return View(randevu);
        }

        randevu.Durum = "Bekliyor";
        _context.Randevular.Add(randevu);
        _context.SaveChanges();
        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> KullaniciRandevular()
    {
        var kullaniciEmail = HttpContext.Session.GetString("KullaniciEmail");

        if (string.IsNullOrEmpty(kullaniciEmail))
        {
            // Eğer kullanıcı giriş yapmamışsa, login sayfasına yönlendir
            return RedirectToAction("Login", "Kullanici");
        }
        int kullaniciId = HttpContext.Session.GetInt32("KullaniciId") ?? 0;
        if (kullaniciId == 0)
        {
            return RedirectToAction("Login", "Kullanici");
        }

        var randevular = await _context.Randevular
            .Include(r => r.Calisan)
            .Include(r => r.Hizmet)
            .Where(r => r.KullaniciId == kullaniciId)
            .OrderBy(r => r.RandevuTarih)
            .ToListAsync();

        return View(randevular);
    }
    public IActionResult Onayla(int randevuId)
    {
        // İlgili randevuyu bul
        var randevu = _context.Randevular.FirstOrDefault(r => r.RandevuId == randevuId);

        if (randevu != null)
        {
            // Durumu güncelle
            randevu.Durum = "Onaylandı";

            // Veritabanını güncelle
            _context.SaveChanges();
        }

        // Aynı sayfaya geri yönlendir
        return RedirectToAction("AdminBekleyenRandevular"); // "Index" yerine ilgili listeleme view'ini belirtin
    }
    public IActionResult Reddet(int randevuId)
    {
        // İlgili randevuyu bul
        var randevu = _context.Randevular.FirstOrDefault(r => r.RandevuId == randevuId);

        if (randevu != null)
        {
            // Durumu güncelle
            randevu.Durum = "Reddedildi";

            // Veritabanını güncelle
            _context.SaveChanges();
        }

        // Aynı sayfaya geri yönlendir
        return RedirectToAction("AdminBekleyenRandevular"); // "Index" yerine ilgili listeleme view'ini belirtin
    }
    public async Task<IActionResult> RandevuListesi()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://localhost:7079/api/RandevuApi/GetAllRandevular");

        if (response.IsSuccessStatusCode)
        {
            var randevular = await response.Content.ReadFromJsonAsync<List<RandevuViewModel>>();
            return View(randevular);
        }

        return View(new List<RandevuViewModel>());
    }
}


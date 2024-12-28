using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_programlama_proje.Models;

namespace web_programlama_proje.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandevuApiController : Controller
    {
        private readonly DataContext _datacontext;

        public RandevuApiController(DataContext context)
        {
            _datacontext = context;
        }

        // GET: RandevuApi/Index
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        // GET: api/RandevuApi/GetAllRandevular
        [HttpGet("GetAllRandevular")]
        public async Task<IActionResult> GetAllRandevular()
        {
            var randevular = await _datacontext.Randevular
                .Include(r => r.Calisan)
                .Include(r => r.Hizmet)
                .Include(r => r.Kullanici)
                .Select(r => new
                {
                    r.RandevuId,
                    RandevuTarih = r.RandevuTarih.ToString("dd.MM.yyyy HH:mm"),
                    r.Durum,
                    CalisanAd = r.Calisan.CalisanAd,
                    HizmetAd = r.Hizmet.HizmetAd,
                    KullaniciAd = r.Kullanici.KullaniciAd
                })
                .ToListAsync();

            return Ok(randevular);
        }
    }
}

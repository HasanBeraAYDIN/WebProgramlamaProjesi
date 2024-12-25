using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_programlama_proje.Models;

namespace web_programlama_proje.Controllers
{
    public class RandevuController : Controller
    {
        private DataContext _dataContext;
        public RandevuController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Randevu()
        {
            ViewBag.Calisanlar = _dataContext.Calisanlar
             .Select(c => new SelectListItem
             {
                 Value = c.CalisanId.ToString(),
                 Text = c.CalisanAd
             })
             .ToList();

            // Hizmetleri ViewBag'e ekle
            ViewBag.Hizmetler = _dataContext.Hizmetler
                .Select(h => new SelectListItem
                {
                    Value = h.HizmetId.ToString(),
                    Text = h.HizmetAd
                })
                .ToList();

            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using wep_programlama_proje.Data;

namespace wep_programlama_proje.Controllers
{
    public class KayitController : Controller
    {
        private DataContext _context;
        public KayitController(DataContext context)
        {
            _context = context;
        }
        public IActionResult Kayit()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Kayit(Data.Musteri model)
        {
            _context.Musteriler.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
            
        }
    }
}
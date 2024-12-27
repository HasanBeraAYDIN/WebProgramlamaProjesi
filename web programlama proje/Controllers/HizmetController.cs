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
            var hizmetler = _dataContext.Hizmetler.ToList(); // Hizmetleri veritabanından al
            return View(hizmetler); // Hizmetleri view'a gönder

        }
    }
}

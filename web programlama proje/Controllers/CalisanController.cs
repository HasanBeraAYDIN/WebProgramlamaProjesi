using Microsoft.AspNetCore.Mvc;

namespace web_programlama_proje.Controllers
{
    public class CalisanController : Controller
    {
        public IActionResult Calisanlar()
        {
            return View();
        }
        public IActionResult CalisanEkle()
        {
            return View();
        }
    }
}

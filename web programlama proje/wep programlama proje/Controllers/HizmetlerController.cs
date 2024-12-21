using Microsoft.AspNetCore.Mvc;

namespace wep_programlama_proje.Controllers
{
    public class HizmetlerController : Controller
    {
        public IActionResult hizmetler()
        {
            return View();
        }
    }
}

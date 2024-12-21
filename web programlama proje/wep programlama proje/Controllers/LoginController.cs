using Microsoft.AspNetCore.Mvc;
using wep_programlama_proje.Data;

namespace wep_programlama_proje.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Login(Musteri model)
        {
            return View(model);
        }

    }
}

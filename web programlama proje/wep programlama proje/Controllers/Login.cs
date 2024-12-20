using Microsoft.AspNetCore.Mvc;

namespace wep_programlama_proje.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}

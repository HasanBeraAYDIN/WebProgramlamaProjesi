using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using web_programlama_proje.Models;

namespace web_programlama_proje.Controllers
{
   // [Route("api/[controller]")]
    //[ApiController]
    public class AdminController : Controller
    {
        private DataContext _dataContext;
        public AdminController(DataContext context)
        {
            _dataContext = context;
        }      

        public IActionResult Calisanlar()
        {
            return View(); 
        }
     
        public IActionResult Admin()
        {
            return View();
        }

        public IActionResult AdminPanel()
        {

           var admin = HttpContext.Session.GetInt32("AdminId");

         /*   if (string.IsNullOrEmpty(admin.ToString()))
            {
                // Eğer kullanıcı giriş yapmamışsa, login sayfasına yönlendir
                return RedirectToAction("Admin", "Admin");
            }

            // Kullanıcı giriş yaptıysa, profil bilgilerini getirebilirsiniz
            var dogrulama = _dataContext.Adminler.FirstOrDefault(k => k.AdminAd ==admin );

            if (dogrulama == null)
            {
                return RedirectToAction("AdminPanel", "Admin");
            }*/

            return View(); // Profil sayfasını kullanıcının bilgileriyle döndür
        }

        [HttpPost]
        public async Task<IActionResult> Admin(string AdminAd, string AdminPassword)
        {
            HttpContext.Session.Clear();

            var admin = await _dataContext.Adminler
       .FirstOrDefaultAsync(k => k.AdminAd == AdminAd);


            if (AdminPassword == admin.AdminPassword)
            {
                HttpContext.Session.SetInt32("id", admin.AdminId);
                return RedirectToAction("AdminPanel", "Admin");
            }

            Console.WriteLine("Şifre hatalı.");
            ModelState.AddModelError("Password", "Şifre yanlış");
            return View();

        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();


            return RedirectToAction("Index", "Home");
        }

    }
}

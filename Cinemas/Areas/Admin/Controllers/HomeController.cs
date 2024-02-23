using Cinemas.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Cinemas.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated) return RedirectToAction("Login", "Accounts");
            var thongtinkh = JsonConvert.DeserializeObject<KhachHang>(HttpContext.Session.GetString("ThongTinKH"));
            if (thongtinkh.RoleId != 1)
            {
                return RedirectToAction("Index", "Phim");
            }
            return View();
        }
    }
}

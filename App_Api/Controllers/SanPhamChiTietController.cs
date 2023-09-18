using Microsoft.AspNetCore.Mvc;

namespace App_Api.Controllers
{
    public class SanPhamChiTietController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

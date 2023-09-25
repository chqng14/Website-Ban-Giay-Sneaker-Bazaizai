using Microsoft.AspNetCore.Mvc;

namespace App_View.Controllers
{
    public class KhuyenMaiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class ElectronicsTab: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

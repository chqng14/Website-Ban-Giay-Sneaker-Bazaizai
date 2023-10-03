using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class ElectronicCategory : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

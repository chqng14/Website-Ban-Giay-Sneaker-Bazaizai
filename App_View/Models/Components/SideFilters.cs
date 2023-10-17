using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class SideFilters: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}

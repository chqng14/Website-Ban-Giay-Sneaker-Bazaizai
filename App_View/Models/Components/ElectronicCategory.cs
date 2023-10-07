using App_Data.DbContextt;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class ElectronicCategory : ViewComponent
    {
        private readonly BazaizaiContext bazaizaiContext;

        public ElectronicCategory(BazaizaiContext bazaizaiContext)
        {
            this.bazaizaiContext = bazaizaiContext;
        }

        public IViewComponentResult Invoke()
        {
            return View(bazaizaiContext.thuongHieus.ToList());
        }
    }
}

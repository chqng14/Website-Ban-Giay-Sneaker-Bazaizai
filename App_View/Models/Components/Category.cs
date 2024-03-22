using App_Data.DbContext;
using Microsoft.AspNetCore.Mvc;

namespace App_View.Models.Components
{
    public class Category : ViewComponent
    {
        private readonly BazaizaiContext bazaizaiContext;

        public Category(BazaizaiContext bazaizaiContext)
        {
            this.bazaizaiContext = bazaizaiContext;
        }

        public IViewComponentResult Invoke()
        {
            return View(bazaizaiContext.ThuongHieus.ToList());
        }
    }
}

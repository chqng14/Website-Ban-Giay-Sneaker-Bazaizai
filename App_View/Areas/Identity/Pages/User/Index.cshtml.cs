using App_Data.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using OpenXmlPowerTools;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace App_View.Areas.Identity.Pages.User
{
    public class IndexModel : PageModel
    {

        private readonly UserManager<NguoiDung> _userManager;

        public IndexModel(UserManager<NguoiDung> userManager)
        {
            _userManager = userManager;
        }
        public class User_Role : NguoiDung
        {
            public string RoleName { get; set; }
        }

        // public List<NguoiDung> users { get; set; }
        public List<User_Role> users { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public const int ITEMS_PER_PAGE = 15;

        [BindProperty(SupportsGet = true, Name = "p")]
        public int currentPage { get; set; }
        public int countPages { get; set; }
        public int totalUsers { get; set; }




        public async Task<IActionResult> OnGet()
        {
            var qr = _userManager.Users.OrderBy(x => x.UserName);
            totalUsers = await qr.CountAsync();
            countPages = (int)Math.Ceiling((double)totalUsers / ITEMS_PER_PAGE);
            if (currentPage < 1)
                currentPage = 1;
            if (currentPage > countPages)
                currentPage = countPages;
            var qr1 = qr.Skip((currentPage - 1) * ITEMS_PER_PAGE)
            .Take(ITEMS_PER_PAGE)
            .Select(x => new User_Role()
            {
                Id = x.Id,
                UserName = x.UserName,
            });
            users = await qr1.ToListAsync();
            foreach(var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                user.RoleName=string.Join(",", roles);
            }

            //users= await  _userManager.Users.OrderBy(x=>x.UserName).ToListAsync();
            return Page();
        }
        //public async Task<IActionResult> OnPostAsync()
        //{
        //    if (Request.Form["addButton"] != StringValues.Empty)
        //    {
        //        for (int i = 0; i < 100; i++)
        //        {
        //            await _userManager.CreateAsync(new NguoiDung { UserName = "user" + i, Email = "user" + i + "@gmail.com" }, "123");
        //        }
        //    }

        //    return Page();
        //}
    }
}

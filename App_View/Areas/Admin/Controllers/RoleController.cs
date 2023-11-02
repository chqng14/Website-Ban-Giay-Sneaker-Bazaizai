using App_Data.Models;
using App_Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ChucVu> _roleManager;

        public RoleController(RoleManager<ChucVu> roleManager)
        {
            _roleManager = roleManager;
        }
        public List<ChucVu> roles { get; set; }
        public ChucVu role { get; set; }


        [TempData]
        public string StatusMessage { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Phải nhập tên role")]
            [Display(Name = "Tên chức vụ")]
            [StringLength(100, ErrorMessage = "{0} dài {2} đến {1} ký tự.", MinimumLength = 3)]
            public string Name { set; get; }
        }

        [BindProperty]
        public InputModel Input { set; get; }

        public async Task<IActionResult> GetAllRole()
        {
            roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }


        public async Task<IActionResult> DetailRole(string id)
        {
            role = await _roleManager.FindByIdAsync(id);
            return View(role);
        }

        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(ChucVu role)
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = null;
                return RedirectToAction("GetAllRole");
            }

            var newRole = new ChucVu();
            newRole.Name = Input.Name;
            newRole.Id = Guid.NewGuid().ToString();
            string MaTS = "CV" + (await _roleManager.Roles.CountAsync() + 1);
            var result = await _roleManager.CreateAsync(newRole);
            if (result.Succeeded)
            {
                StatusMessage = $"Tạo thành công chức vụ mới : {newRole.Name}";
                return RedirectToAction("GetAllRole");
            }
            else
            {
                result.Errors.ToList().ForEach(error =>
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                });
            }
            return RedirectToAction("GetAllRole");
        }

        // POST: RoleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RoleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: RoleController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RoleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

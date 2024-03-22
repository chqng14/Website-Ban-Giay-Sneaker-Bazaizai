using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContext;
using App_Data.Models;
using Newtonsoft.Json;
using System.Net.Http;
using DocumentFormat.OpenXml.Drawing;
using Org.BouncyCastle.Ocsp;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using static App_Data.Repositories.TrangThai;
using App_View.IServices;
using App_View.Services;
using App_Data.Repositories;
using Google.Apis.PeopleService.v1.Data;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using Microsoft.AspNetCore.Authorization;
using MimeKit;
using System.Net.Http.Headers;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class KhuyenMaisController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly HttpClient _httpClient;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly IViewRenderService _viewRenderService;
        public KhuyenMaisController(BazaizaiContext context, IEmailSender emailSender, SignInManager<NguoiDung> signInManager, UserManager<NguoiDung> userManager, IViewRenderService viewRenderService)
        {
            _context = context;
            _httpClient = new HttpClient();
            _emailSender = emailSender;
            _signInManager = signInManager;
            _userManager = userManager;
            _viewRenderService = viewRenderService;
        }

        // GET: Admin/KhuyenMais
        public async Task<IActionResult> Index()
        {
            if (TempData["ThongBao"] != null)
            {
                ViewBag.ThongBao = TempData["ThongBao"].ToString();
            }
            var KhuyenMais = JsonConvert.DeserializeObject<List<KhuyenMai>>(await (await _httpClient.GetAsync("https://localhost:7038/api/KhuyenMai")).Content.ReadAsStringAsync());
            return View(KhuyenMais);
        }
        public async Task<IActionResult> LstSaleAsync(string trangThaiSale,string loaiHinhKM, string tenKM)
        {
            var KhuyenMais = JsonConvert.DeserializeObject<List<KhuyenMai>>(await (await _httpClient.GetAsync("https://localhost:7038/api/KhuyenMai")).Content.ReadAsStringAsync());
            if(!string.IsNullOrEmpty(trangThaiSale))
            {
                KhuyenMais = KhuyenMais.Where(x => x.TrangThai == Convert.ToInt32(trangThaiSale)).ToList();
            }
            if (!string.IsNullOrEmpty(loaiHinhKM))
            {
                KhuyenMais = KhuyenMais.Where(x => x.LoaiHinhKM == Convert.ToInt32(loaiHinhKM)).ToList();
            }
            if (!string.IsNullOrEmpty(tenKM))
            {
                KhuyenMais = KhuyenMais.Where(x => x.TenKhuyenMai.ToUpper().Contains(tenKM.ToUpper())).ToList();
            }
            return PartialView("_LstSale",KhuyenMais);
        }
        // GET: Admin/KhuyenMais/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.KhuyenMais == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais
                .FirstOrDefaultAsync(m => m.IdKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Create
        public IActionResult Create()
        {
            ViewBag.ListLoaiHinh = new List<SelectListItem>
            {
                new SelectListItem { Text = "Khuyến mại giảm giá", Value = "1" },
                new SelectListItem { Text = "Khuyến mãi đồng giá", Value = "0" }
            };
            ViewBag.ListGiamGia = new List<SelectListItem>
            {
                new SelectListItem { Text = "5%", Value = "5" },
                new SelectListItem { Text = "10%", Value = "10" },
                new SelectListItem { Text = "15%", Value = "15" },
                new SelectListItem { Text = "20%", Value = "20" },
                new SelectListItem { Text = "25%", Value = "25" },
                new SelectListItem { Text = "30%", Value = "30" },
                new SelectListItem { Text = "35%", Value = "35" },
                new SelectListItem { Text = "40%", Value = "40" },
                new SelectListItem { Text = "45%", Value = "45" },
                new SelectListItem { Text = "50%", Value = "50" },
                new SelectListItem { Text = "55%", Value = "55" },
                new SelectListItem { Text = "60%", Value = "60" },
                new SelectListItem { Text = "65%", Value = "65" },
                new SelectListItem { Text = "70%", Value = "70" },
                new SelectListItem { Text = "75%", Value = "75" },
                new SelectListItem { Text = "80%", Value = "80" },
                new SelectListItem { Text = "85%", Value = "85" },
                new SelectListItem { Text = "90%", Value = "90" },
                new SelectListItem { Text = "95%", Value = "95" },
                new SelectListItem { Text = "100%", Value = "100" }
            };
            return View();
        }

        // POST: Admin/KhuyenMais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuyenMai khuyenMai,IFormFile formFile)
        {
            khuyenMai.IdKhuyenMai = Guid.NewGuid().ToString();
            khuyenMai.TrangThai = 0;
            ViewData["IdKhuyenMai"] = new SelectList(_context.KhuyenMais, "IdKhuyenMai", "IdKhuyenMai");
            ViewBag.ListLoaiHinh = new List<SelectListItem>
            {
                new SelectListItem { Text = "Khuyến mại giảm giá", Value = "1" },
                new SelectListItem { Text = "Khuyến mãi đồng giá", Value = "0" }
            };
            ViewBag.ListGiamGia = new List<SelectListItem>
            {
                new SelectListItem { Text = "5%", Value = "5" },
                new SelectListItem { Text = "10%", Value = "10" },
                new SelectListItem { Text = "15%", Value = "15" },
                new SelectListItem { Text = "20%", Value = "20" },
                new SelectListItem { Text = "25%", Value = "25" },
                new SelectListItem { Text = "30%", Value = "30" },
                new SelectListItem { Text = "35%", Value = "35" },
                new SelectListItem { Text = "40%", Value = "40" },
                new SelectListItem { Text = "45%", Value = "45" },
                new SelectListItem { Text = "50%", Value = "50" },
                new SelectListItem { Text = "55%", Value = "55" },
                new SelectListItem { Text = "60%", Value = "60" },
                new SelectListItem { Text = "65%", Value = "65" },
                new SelectListItem { Text = "70%", Value = "70" },
                new SelectListItem { Text = "75%", Value = "75" },
                new SelectListItem { Text = "80%", Value = "80" },
                new SelectListItem { Text = "85%", Value = "85" },
                new SelectListItem { Text = "90%", Value = "90" },
                new SelectListItem { Text = "95%", Value = "95" },
                new SelectListItem { Text = "100%", Value = "100" }
            };
            try
            {
                if (khuyenMai.TenKhuyenMai != null&& khuyenMai.NgayBatDau != null&& khuyenMai.NgayKetThuc != null&& khuyenMai.MucGiam != null && khuyenMai.LoaiHinhKM != null&&khuyenMai.NgayBatDau<=khuyenMai.NgayKetThuc)
                {

                    using var content = new MultipartFormDataContent();
                    content.Add(new StringContent($"{khuyenMai.LoaiHinhKM}"), "LoaiHinhKM");
                    content.Add(new StringContent($"{khuyenMai.MucGiam}"), "MucGiam");
                    content.Add(new StringContent($"{khuyenMai.NgayKetThuc?.ToString("dd/MM/yyyy hh:mm:ss tt")}"), "NgayKetThuc");
                    content.Add(new StringContent($"{khuyenMai.TenKhuyenMai}"), "TenKhuyenMai");
                    content.Add(new StringContent($"{khuyenMai.IdKhuyenMai}"), "IdKhuyenMai");
                    content.Add(new StringContent($"{khuyenMai.NgayBatDau?.ToString("dd/MM/yyyy hh:mm:ss tt")}"), "NgayBatDau");
                    content.Add(new StringContent("111"), "MaKhuyenMai");
                    content.Add(new StringContent($"{khuyenMai.TrangThai}"), "TrangThai");
                    if (formFile != null && formFile.Length > 0)
                    {

                        var streamContent = new StreamContent(formFile.OpenReadStream());
                        streamContent.Headers.Add("Content-Type", formFile.ContentType);
                        content.Add(streamContent, "formFile", formFile.FileName);
                        var response = await _httpClient.PostAsync($"https://localhost:7038/api/KhuyenMai/Create-KhuyenMai", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var lstUser = await _userManager.GetUsersInRoleAsync(ChucVuMacDinh.KhachHang.ToString());
                            var KM = _context.KhuyenMais.Find(khuyenMai.IdKhuyenMai);
                            var htmlMessage = await _viewRenderService.RenderToStringAsync("KhuyenMai/ViewMail", KM);
                            foreach (var user in lstUser)
                            {
                                await _emailSender.SendEmailAsync(user.Email, "Khuyến mại 100%",
                                 htmlMessage);
                            }
                         
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            Console.WriteLine(response.StatusCode);
                            return BadRequest();
                        }
                    }
                    else {
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
                return View();
            }

            return View();
        }

        // GET: Admin/KhuyenMais/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.ListLoaiHinh = new List<SelectListItem>
            {
                new SelectListItem { Text = "Khuyến mại giảm giá", Value = "1" },
                new SelectListItem { Text = "Khuyến mãi đồng giá", Value = "0" }
            };
            ViewBag.TrangThai = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hết hạn", Value = "0" },
                new SelectListItem { Text = "Đang hoạt động", Value = "1" },
                 new SelectListItem { Text = "Chưa bắt đầu", Value = "2" },
                  new SelectListItem { Text = "Buộc dừng", Value = "3" }
            };
            ViewBag.ListGiamGia = new List<SelectListItem>
            {
                new SelectListItem { Text = "5%", Value = "5" },
                new SelectListItem { Text = "10%", Value = "10" },
                new SelectListItem { Text = "15%", Value = "15" },
                new SelectListItem { Text = "20%", Value = "20" },
                new SelectListItem { Text = "25%", Value = "25" },
                new SelectListItem { Text = "30%", Value = "30" },
                new SelectListItem { Text = "35%", Value = "35" },
                new SelectListItem { Text = "40%", Value = "40" },
                new SelectListItem { Text = "45%", Value = "45" },
                new SelectListItem { Text = "50%", Value = "50" },
                new SelectListItem { Text = "55%", Value = "55" },
                new SelectListItem { Text = "60%", Value = "60" },
                new SelectListItem { Text = "65%", Value = "65" },
                new SelectListItem { Text = "70%", Value = "70" },
                new SelectListItem { Text = "75%", Value = "75" },
                new SelectListItem { Text = "80%", Value = "80" },
                new SelectListItem { Text = "85%", Value = "85" },
                new SelectListItem { Text = "90%", Value = "90" },
                new SelectListItem { Text = "95%", Value = "95" },
                new SelectListItem { Text = "100%", Value = "100" }
            };
            if (id == null || _context.KhuyenMais == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais.FirstOrDefaultAsync(x=>x.IdKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, KhuyenMai khuyenMai,IFormFile formFile)
        {
            ViewBag.ListLoaiHinh = new List<SelectListItem>
            {
                new SelectListItem { Text = "Khuyến mại giảm giá", Value = "1" },
                new SelectListItem { Text = "Khuyến mãi đồng giá", Value = "0" }
            };
            ViewBag.TrangThai = new List<SelectListItem>
            {
                new SelectListItem { Text = "Hết hạn", Value = "0" },
                new SelectListItem { Text = "Đang hoạt động", Value = "1" },
                 new SelectListItem { Text = "Chưa bắt đầu", Value = "2" },
                  new SelectListItem { Text = "Buộc dừng", Value = "3" }
            };
            ViewBag.ListGiamGia = new List<SelectListItem>
            {
                new SelectListItem { Text = "5%", Value = "5" },
                new SelectListItem { Text = "10%", Value = "10" },
                new SelectListItem { Text = "15%", Value = "15" },
                new SelectListItem { Text = "20%", Value = "20" },
                new SelectListItem { Text = "25%", Value = "25" },
                new SelectListItem { Text = "30%", Value = "30" },
                new SelectListItem { Text = "35%", Value = "35" },
                new SelectListItem { Text = "40%", Value = "40" },
                new SelectListItem { Text = "45%", Value = "45" },
                new SelectListItem { Text = "50%", Value = "50" },
                new SelectListItem { Text = "55%", Value = "55" },
                new SelectListItem { Text = "60%", Value = "60" },
                new SelectListItem { Text = "65%", Value = "65" },
                new SelectListItem { Text = "70%", Value = "70" },
                new SelectListItem { Text = "75%", Value = "75" },
                new SelectListItem { Text = "80%", Value = "80" },
                new SelectListItem { Text = "85%", Value = "85" },
                new SelectListItem { Text = "90%", Value = "90" },
                new SelectListItem { Text = "95%", Value = "95" },
                new SelectListItem { Text = "100%", Value = "100" }
            };
   

           if (khuyenMai.TenKhuyenMai != null && khuyenMai.NgayBatDau != null && khuyenMai.NgayKetThuc != null && khuyenMai.MucGiam != null && khuyenMai.LoaiHinhKM != null && khuyenMai.NgayBatDau <= khuyenMai.NgayKetThuc)
                {

                    using var content = new MultipartFormDataContent();
                    content.Add(new StringContent($"{khuyenMai.LoaiHinhKM}"), "LoaiHinhKM");
                    content.Add(new StringContent($"{khuyenMai.MucGiam}"), "MucGiam");
                    content.Add(new StringContent($"{khuyenMai.NgayKetThuc?.ToString("dd/MM/yyyy hh:mm:ss tt")}"), "NgayKetThuc");
                    content.Add(new StringContent($"{khuyenMai.TenKhuyenMai}"), "TenKhuyenMai");
                    content.Add(new StringContent($"{id}"), "IdKhuyenMai");
                    content.Add(new StringContent($"{khuyenMai.NgayBatDau?.ToString("dd/MM/yyyy hh:mm:ss tt")}"), "NgayBatDau");
                    content.Add(new StringContent("111"), "MaKhuyenMai");
                    content.Add(new StringContent($"{khuyenMai.TrangThai}"), "TrangThai");
                    if (formFile != null && formFile.Length > 0)
                        {

                            var streamContent = new StreamContent(formFile.OpenReadStream());
                            streamContent.Headers.Add("Content-Type", formFile.ContentType);
                            content.Add(streamContent, "formFile", formFile.FileName);
                            string urlApi = $"https://localhost:7038/api/KhuyenMai/Edit";
                            var response = await _httpClient.PutAsync(urlApi, content);
                                if (response.IsSuccessStatusCode)
                                {

                                    return RedirectToAction("Index");
                                }
                                else
                                {
                                    Console.WriteLine(response.StatusCode);
                                    return BadRequest();
                                }
                            }
                            else {
                            khuyenMai.IdKhuyenMai = id;
                            content.Add(new StringContent($"{khuyenMai.LoaiHinhKM}"), "LoaiHinhKM");
                            content.Add(new StringContent($"{khuyenMai.MucGiam}"), "MucGiam");
                            content.Add(new StringContent($"{khuyenMai.NgayKetThuc?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}"), "NgayKetThuc");
                            content.Add(new StringContent($"{khuyenMai.TenKhuyenMai}"), "TenKhuyenMai");
                            content.Add(new StringContent($"{id}"), "IdKhuyenMai");
                            content.Add(new StringContent($"{khuyenMai.NgayBatDau?.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")}"), "NgayBatDau");
                            content.Add(new StringContent("111"), "MaKhuyenMai");
                            content.Add(new StringContent($"{khuyenMai.TrangThai}"), "TrangThai");
                            var response = await _httpClient.PutAsync($"https://localhost:7038/api/KhuyenMai/EditNoiImage", content);
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                Console.WriteLine(response.StatusCode);
                                return BadRequest();
                            }
                        }
                    }
            return View(khuyenMai);
        }

        // GET: Admin/KhuyenMais/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.KhuyenMais == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais
                .FirstOrDefaultAsync(m => m.IdKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        // POST: Admin/KhuyenMais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.KhuyenMais == null)
            {
                return Problem("Entity set 'BazaizaiContext.KhuyenMais'  is null.");
            }
            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai != null)
            {
                _context.KhuyenMais.Remove(khuyenMai);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhuyenMaiExists(string id)
        {
            return (_context.KhuyenMais?.Any(e => e.IdKhuyenMai == id)).GetValueOrDefault();
        }
        [HttpPost]
        public JsonResult CapNhatTrangThai(string id, int trangThai)
        {
            var khuyenMai = _context.KhuyenMais.Find(id);
            string mess;
            if (trangThai == (int)TrangThaiSale.BuocDung)
            {
				if(khuyenMai.NgayKetThuc >= DateTime.Now&&khuyenMai.NgayBatDau<= DateTime.Now)
				{

					khuyenMai.TrangThai = (int)TrangThaiSale.DangBatDau;
				}
                else
                    if(khuyenMai.NgayBatDau >= DateTime.Now)
                {
                    return Json(new { success = false, mess= "Khuyến mại chưa đến ngày bắt đầu" });
                }
                else return Json(new { success = false, mess = "Chương trình khuyến mãi đã hết hạn" });

            }
            else
            {
                khuyenMai.TrangThai = (int)TrangThaiSale.BuocDung;
            }
            _context.KhuyenMais.Update(khuyenMai);
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}

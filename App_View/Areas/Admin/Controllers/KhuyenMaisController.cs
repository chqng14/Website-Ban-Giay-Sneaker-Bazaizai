using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContextt;
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
using System.Text;
using Azure.Storage.Blobs;
using Path = System.IO.Path;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var KhuyenMais = JsonConvert.DeserializeObject<List<KhuyenMai>>(await (await _httpClient.GetAsync("https://bazaizaistoreapi.azurewebsites.net/api/KhuyenMai")).Content.ReadAsStringAsync());
            string hostName = HttpContext.Request.Host.Value;
            if (hostName.ToLower().Contains("localhost"))
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string rootPath = Directory.GetParent(currentDirectory).FullName;
                string uploadDirectory = Path.Combine(rootPath, "App_View", "wwwroot", "AnhSale");
                foreach (var item in KhuyenMais)
                {
                    string imagePath = Path.Combine(uploadDirectory, item.Url);
                    if (!System.IO.File.Exists(imagePath))
                    {
                        item.Url = "host.png";
                    }
                }
            }
            else
            {
                var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=azurenhucut;AccountKey=YXdnVGGVxA8rYNZQrNMfIu8ZDI47+/wZYr2ypN4vmp8TynAzJ4xXoq9kizECI4CkWtyJmpoT6veG+AStGLH21g==;EndpointSuffix=core.windows.net");
                var containerClient = blobServiceClient.GetBlobContainerClient("anhsale");
                foreach (var item in KhuyenMais)
                {
                    var blobClient = containerClient.GetBlobClient(item.Url);
                    if (await blobClient.ExistsAsync() && blobClient.Name == item.Url)
                    {
                        var blobDownloadInfo = await blobClient.DownloadAsync();
                        using (var streamReader = new StreamReader(blobDownloadInfo.Value.Content))
                        {
                            byte[] imageBytes = new byte[blobDownloadInfo.Value.ContentLength];
                            int bytesRead = 0;
                            while (bytesRead < imageBytes.Length)
                            {
                                bytesRead += await streamReader.BaseStream.ReadAsync(imageBytes, bytesRead, imageBytes.Length - bytesRead);
                            }

                            string azureSitePath = Environment.GetEnvironmentVariable("HOME");
                            string uploadDirectory = Path.Combine(azureSitePath, "site", "wwwroot", "wwwroot", "AnhSale");

                            // Lưu ảnh vào thư mục
                            string imagePath = Path.Combine(uploadDirectory, item.Url);
                            await System.IO.File.WriteAllBytesAsync(imagePath, imageBytes);
                        }
                    }
                    else
                    {
                        item.Url = "local.png";
                    }
                }
            }

            return View(KhuyenMais);
        }
        public async Task<IActionResult> LstSaleAsync(string trangThaiSale, string loaiHinhKM, string tenKM)
        {
            var KhuyenMais = JsonConvert.DeserializeObject<List<KhuyenMai>>(await (await _httpClient.GetAsync("https://bazaizaistoreapi.azurewebsites.net/api/KhuyenMai")).Content.ReadAsStringAsync());
            if (!string.IsNullOrEmpty(trangThaiSale))
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
            return PartialView("_LstSale", KhuyenMais);
        }
        // GET: Admin/KhuyenMais/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.khuyenMais == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.khuyenMais
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
        public async Task<IActionResult> Create(KhuyenMai khuyenMai, IFormFile formFile)
        {
            khuyenMai.IdKhuyenMai = Guid.NewGuid().ToString();
            khuyenMai.TrangThai = 0;
            ViewData["IdKhuyenMai"] = new SelectList(_context.khuyenMais, "IdKhuyenMai", "IdKhuyenMai");
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
                if (khuyenMai.TenKhuyenMai != null && khuyenMai.NgayBatDau != null && khuyenMai.NgayKetThuc != null && khuyenMai.MucGiam != null && khuyenMai.LoaiHinhKM != null)
                {

                    using var content = new MultipartFormDataContent();
                    if (formFile != null && formFile.Length > 0)
                    {

                        var streamContent = new StreamContent(formFile.OpenReadStream());
                        streamContent.Headers.Add("Content-Type", formFile.ContentType);
                        content.Add(streamContent, "formFile", formFile.FileName);
                        var response = await _httpClient.PostAsync($"https://bazaizaistoreapi.azurewebsites.net/api/KhuyenMai/Create-KhuyenMai?id={khuyenMai.IdKhuyenMai}&Ten={khuyenMai.TenKhuyenMai}&ngayBD={khuyenMai.NgayBatDau}&ngayKT={khuyenMai.NgayKetThuc}&trangThai={khuyenMai.TrangThai}&mucGiam={khuyenMai.MucGiam}&loaiHinh={khuyenMai.LoaiHinhKM}", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var lstUser = await _userManager.GetUsersInRoleAsync(ChucVuMacDinh.KhachHang.ToString());
                            var KM = _context.khuyenMais.Find(khuyenMai.IdKhuyenMai);
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
                    else
                    {
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
            if (id == null || _context.khuyenMais == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.khuyenMais.FirstOrDefaultAsync(x => x.IdKhuyenMai == id);
            var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=azurenhucut;AccountKey=YXdnVGGVxA8rYNZQrNMfIu8ZDI47+/wZYr2ypN4vmp8TynAzJ4xXoq9kizECI4CkWtyJmpoT6veG+AStGLH21g==;EndpointSuffix=core.windows.net");
            var containerClient = blobServiceClient.GetBlobContainerClient("anhsale");
            var blobClient = containerClient.GetBlobClient(khuyenMai.Url);
            if (!(await blobClient.ExistsAsync()) || blobClient.Name != khuyenMai.Url)
            {
                khuyenMai.Url = "local.png";
            }
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
        public async Task<IActionResult> Edit(string id, KhuyenMai khuyenMai, IFormFile formFile)
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


            if (khuyenMai.TenKhuyenMai != null && khuyenMai.NgayBatDau != null && khuyenMai.NgayKetThuc != null && khuyenMai.MucGiam != null && khuyenMai.LoaiHinhKM != null)
            {

                using var content = new MultipartFormDataContent();
                //content.Add(new StringContent(khuyenMai.TrangThai.ToString()), "trangThai");
                //content.Add(new StringContent(khuyenMai.TenKhuyenMai), "Ten");
                //content.Add(new StringContent(khuyenMai.NgayBatDau.ToString()), "ngayBD");
                //content.Add(new StringContent(khuyenMai.NgayKetThuc.ToString()), "ngayKT");
                //content.Add(new StringContent(khuyenMai.LoaiHinhKM.ToString()), "loaiHinh");
                //content.Add(new StringContent(khuyenMai.PhamVi), "PhamVi");
                //content.Add(new StringContent(khuyenMai.MucGiam.ToString()), "mucGiam");
                if (formFile != null && formFile.Length > 0)
                {

                    var streamContent = new StreamContent(formFile.OpenReadStream());
                    streamContent.Headers.Add("Content-Type", formFile.ContentType);
                    content.Add(streamContent, "formFile", formFile.FileName);
                    var response = await _httpClient.PutAsync($"https://bazaizaistoreapi.azurewebsites.net/api/KhuyenMai/{id}?Ten={khuyenMai.TenKhuyenMai}&ngayBD={khuyenMai.NgayBatDau}&ngayKT={khuyenMai.NgayKetThuc}&trangThai={khuyenMai.TrangThai}&mucGiam={khuyenMai.MucGiam}&loaiHinh={khuyenMai.LoaiHinhKM}", content);
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
                else
                {
                    khuyenMai.IdKhuyenMai = id;
                    var response = await _httpClient.PutAsync($"https://bazaizaistoreapi.azurewebsites.net/api/KhuyenMai/EditNoiImage?id={id}&Ten={khuyenMai.TenKhuyenMai}&ngayBD={khuyenMai.NgayBatDau}&ngayKT={khuyenMai.NgayKetThuc}&trangThai={khuyenMai.TrangThai}&mucGiam={khuyenMai.MucGiam}&loaiHinh={khuyenMai.LoaiHinhKM}", null);
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
            if (id == null || _context.khuyenMais == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.khuyenMais
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
            if (_context.khuyenMais == null)
            {
                return Problem("Entity set 'BazaizaiContext.khuyenMais'  is null.");
            }
            var khuyenMai = await _context.khuyenMais.FindAsync(id);
            if (khuyenMai != null)
            {
                _context.khuyenMais.Remove(khuyenMai);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhuyenMaiExists(string id)
        {
            return (_context.khuyenMais?.Any(e => e.IdKhuyenMai == id)).GetValueOrDefault();
        }
        [HttpPost]
        public JsonResult CapNhatTrangThai(string id, int trangThai)
        {
            var khuyenMai = _context.khuyenMais.Find(id);
            if (trangThai == (int)TrangThaiSale.BuocDung)
            {
                if (khuyenMai.NgayKetThuc >= DateTime.Now)
                {

                    khuyenMai.TrangThai = (int)TrangThaiSale.DangBatDau;
                }
                else return Json(new { success = false });
            }
            else
            {
                khuyenMai.TrangThai = (int)TrangThaiSale.BuocDung;
            }
            _context.khuyenMais.Update(khuyenMai);
            _context.SaveChanges();
            return Json(new { success = true });
        }
    }
}

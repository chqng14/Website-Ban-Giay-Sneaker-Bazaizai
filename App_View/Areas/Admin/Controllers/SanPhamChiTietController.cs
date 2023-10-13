using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContextt;
using App_Data.Models;
using App_View.IServices;
using App_Data.ViewModels.Anh;
using System.Security.Cryptography;
using App_Data.ViewModels.SanPhamChiTiet.SanPhamDTO;
using App_Data.ViewModels.SanPhamChiTiet.ThuongHieuDTO;
using App_Data.ViewModels.XuatXu;
using App_Data.ViewModels.ChatLieuDTO;
using App_Data.ViewModels.LoaiGiayDTO;
using App_Data.ViewModels.KieuDeGiayDTO;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.KichCoDTO;
using App_Data.ViewModels.SanPhamChiTietDTO;
using OfficeOpenXml;
using System.Diagnostics;
using App_View.Models.DTO;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamChiTietController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly ISanPhamChiTietService _sanPhamChiTietService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SanPhamChiTietController(ISanPhamChiTietService sanPhamChiTietService, IWebHostEnvironment webHostEnvironment)
        {
            _context = new BazaizaiContext();
            _sanPhamChiTietService = sanPhamChiTietService;
            _webHostEnvironment = webHostEnvironment;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
        [HttpGet]
        // GET: Admin/SanPhamChiTiet/DanhSachSanPham
        public IActionResult DanhSachSanPham()
        {
            ViewData["ThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "TenThuongHieu");
            ViewData["KichCo"] = new SelectList(_context.kichCos.OrderBy(x=>x.SoKichCo).ToList(), "IdKichCo", "SoKichCo");
            ViewData["MauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "TenMauSac");
            ViewData["SanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "TenSanPham");
            return View();
        }


        // GET: Admin/SanPhamChiTiet/DanhSachSanPhamNgungKinhDoanh
        public IActionResult DanhSachSanPhamNgungKinhDoanh()
        {
            return View();
        }

        public class ListGuildDTO
        {
            public List<string>? listGuild { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> NgungKinhDoanhListSanPham([FromBody] ListGuildDTO listGuildDTO)
        {
            return Ok(await _sanPhamChiTietService.NgungKinhDoanhSanPhamAynsc(listGuildDTO));
        }

        [HttpPost]
        public async Task<IActionResult> KinhDoanhLaiListSanPham([FromBody] ListGuildDTO listGuildDTO)
        {
            return Ok(await _sanPhamChiTietService.KinhDoanhLaiSanPhamAynsc(listGuildDTO));
        }

        [HttpGet]
        public async Task<IActionResult> KhoiPhucKinhDoanh(string id)
        {
            return Ok(await _sanPhamChiTietService.KhoiPhucKinhDoanhAynsc(id));
        }

        #region ImportExcel
        [HttpPost]
        public async Task<ActionResult> ImportProducts(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    int slSuccess = 0;
                    using (var stream = file.OpenReadStream())
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;


                        for (int row = 2; row <= rowCount; row++)
                        {
                            var sanpham = worksheet.Cells[row, 1].Text;
                            var thuongHieu = worksheet.Cells[row, 2].Text;
                            var xuatXu = worksheet.Cells[row, 3].Text;
                            var chatLieu = worksheet.Cells[row, 4].Text;
                            var loaiGiay = worksheet.Cells[row, 5].Text;
                            var kieuDeGiay = worksheet.Cells[row, 6].Text;
                            var mauSac = worksheet.Cells[row, 7].Text;
                            var kichCo = worksheet.Cells[row, 8].Text;
                            var giaNhap = worksheet.Cells[row, 9].Text;
                            var giaBan = worksheet.Cells[row, 10].Text;
                            var soLuong = worksheet.Cells[row, 11].Text;
                            var khoiLuong = worksheet.Cells[row, 12].Text;
                            var day = worksheet.Cells[row, 13].Text;
                            var noiBat = worksheet.Cells[row, 14].Text;
                            var listTenAnh = worksheet.Cells[row, 15].Text.Split(',');
                            
                            var sanPhamDTO = await _sanPhamChiTietService.GetItemExcelAynsc(new BienTheDTO
                            {
                                ChatLieu = chatLieu,
                                KichCo = kichCo,
                                KieuDeGiay = kieuDeGiay,
                                LoaiGiay = loaiGiay,
                                MauSac = mauSac,
                                SanPham = sanpham,
                                ThuongHieu = thuongHieu,
                                XuatXu = xuatXu,
                            });
                            sanPhamDTO.GiaNhap = Convert.ToDouble(giaNhap);
                            sanPhamDTO.SoLuongTon = Convert.ToInt32(soLuong);
                            sanPhamDTO.GiaBan = Convert.ToDouble(giaBan);
                            sanPhamDTO.KhoiLuong = Convert.ToDouble(khoiLuong);
                            sanPhamDTO.Day = day == "1" ? true : false;
                            sanPhamDTO.NoiBat = noiBat == "1" ? true : false;
                            var response = (await _sanPhamChiTietService.AddAysnc(sanPhamDTO));

                            if (response.Success)
                            {
                                slSuccess++;
                                foreach (var item in listTenAnh)
                                {
                                     _context.Anh.Add(new Anh()
                                    {
                                        IdAnh = Guid.NewGuid().ToString(),
                                        TrangThai = 0,
                                        Url = item,
                                        IdSanPhamChiTiet = response.IdChiTietSp,
                                    });
                                    _context.SaveChanges();

                                }
                               
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            

            return Ok();

        }

        #endregion


        #region DownLoadFile
        public IActionResult DownloadFile()
        {
            string relativePath = "excel/template.xlsx";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            string fileName = "template.xlsx";

            if (System.IO.File.Exists(filePath))
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                return NotFound();
            }
        }

        #endregion

        #region ExportToExcel
        public async Task<IActionResult> ExportToExcel()
        {
            var lstProduct = await _sanPhamChiTietService.GetListSanPhamExcelAynsc();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ProductList");

                using (var range = worksheet.Cells[1, 1, 1, 15])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.Font.Size = 12;
                }

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "Tên Sản Phẩm";
                worksheet.Cells[1, 3].Value = "Thương hiệu";
                worksheet.Cells[1, 4].Value = "Xuất xứ";
                worksheet.Cells[1, 5].Value = "Chất liệu";
                worksheet.Cells[1, 6].Value = "Loại giầy";
                worksheet.Cells[1, 7].Value = "Kiểu để giầy";
                worksheet.Cells[1, 8].Value = "Màu sắc";
                worksheet.Cells[1, 9].Value = "Kích cỡ";
                worksheet.Cells[1, 10].Value = "Giá nhập";
                worksheet.Cells[1, 11].Value = "Giá bán";
                worksheet.Cells[1, 12].Value = "Số lượng";
                worksheet.Cells[1, 13].Value = "Khối lượng";
                worksheet.Cells[1, 14].Value = "Dây";
                worksheet.Cells[1, 15].Value = "Nổi bật";

                for (int i = 0; i < lstProduct.Count(); i++)
                {
                    worksheet.Cells[i + 2, 1].Value = lstProduct[i].IdChiTietSp;
                    worksheet.Cells[i + 2, 2].Value = lstProduct[i].SanPham;
                    worksheet.Cells[i + 2, 3].Value = lstProduct[i].ThuongHieu;
                    worksheet.Cells[i + 2, 4].Value = lstProduct[i].XuatXu;
                    worksheet.Cells[i + 2, 5].Value = lstProduct[i].ChatLieu;
                    worksheet.Cells[i + 2, 6].Value = lstProduct[i].LoaiGiay;
                    worksheet.Cells[i + 2, 7].Value = lstProduct[i].KieuDeGiay;
                    worksheet.Cells[i + 2, 8].Value = lstProduct[i].MauSac;
                    worksheet.Cells[i + 2, 9].Value = lstProduct[i].KichCo;
                    worksheet.Cells[i + 2, 10].Value = lstProduct[i].GiaNhap;
                    worksheet.Cells[i + 2, 11].Value = lstProduct[i].GiaBan;
                    worksheet.Cells[i + 2, 12].Value = lstProduct[i].SoLuongTon;
                    worksheet.Cells[i + 2, 13].Value = lstProduct[i].KhoiLuong;
                    worksheet.Cells[i + 2, 14].Value = lstProduct[i].Day;
                    worksheet.Cells[i + 2, 15].Value = lstProduct[i].NoiBat;
                }

                byte[] excelBytes = package.GetAsByteArray();

                string fileName = $"ProductList_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> GetPartialViewListUpdate([FromBody] ListGuildDTO listGuildDTO)
        {
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "Ten");
            var model = await _sanPhamChiTietService.GetListSanPhamChiTietDTOAsync(listGuildDTO);
            return PartialView("_DanhSachSanPhamUpdate", model);
        }

        public async Task<IActionResult> GetDanhSachSanPham([FromBody]FilterAdminDTO filterAdminDTO)
        {
            
            var query = (await _sanPhamChiTietService.GetListSanPhamChiTietViewModelAsync());
                

            if (!string.IsNullOrEmpty(filterAdminDTO.searchValue))
            {
                string searchValueLower = filterAdminDTO.searchValue.ToLower();
                query = query.Where(x =>
                x.SanPham!.ToLower().Contains(searchValueLower) ||
                x.LoaiGiay!.ToLower().Contains(searchValueLower) ||
                x.ChatLieu!.ToLower().Contains(searchValueLower) ||
                x.KieuDeGiay!.ToLower().Contains(searchValueLower)
                )
                .ToList();
            }

            if (!string.IsNullOrEmpty(filterAdminDTO.SanPham))
            {
                query = query.Where(x =>x.SanPham!.ToLower().Contains(filterAdminDTO.SanPham.ToLower()))
                .ToList();
            }

            if (!string.IsNullOrEmpty(filterAdminDTO.MauSac))
            {
                Console.WriteLine(filterAdminDTO.MauSac);
                query = query.Where(x => x.MauSac!.ToLower().Contains(filterAdminDTO.MauSac.ToLower()))
                .ToList();
            }

            if (!string.IsNullOrEmpty(filterAdminDTO.ThuongHieu))
            {
                query = query.Where(x => x.SanPham!.ToLower().Contains(filterAdminDTO.ThuongHieu.ToLower()))
                .ToList();
            }

            if (!string.IsNullOrEmpty(filterAdminDTO.KichCo))
            {
                query = query.Where(x => x.KichCo == Convert.ToInt16(filterAdminDTO.KichCo))
                .ToList();
            }

            if (!string.IsNullOrEmpty(filterAdminDTO.Sort))
            {
                if (filterAdminDTO.Sort == "ascending_quantity")
                {
                    query = query.OrderBy(x => x.SoLuongTon!).ToList();
                }

                if (filterAdminDTO.Sort == "descending_quantity")
                {
                    query = query.OrderByDescending(x => x.SoLuongTon!).ToList();
                }

                if (filterAdminDTO.Sort == "ascending_price")
                {
                    query = query.OrderBy(x => x.GiaBan!).ToList();
                }

                if (filterAdminDTO.Sort == "descending_price")
                {
                    query = query.OrderByDescending(x => x.GiaBan!).ToList();
                }
            }

            var totalRecords = query.Count;
            query = query
                .Skip(filterAdminDTO.start)
                .Take(filterAdminDTO.length)
                .ToList();
            return Json(new
            {
                draw = filterAdminDTO.draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = query
            });
        }

        public async Task<IActionResult> GetDanhSachSanPhamNgungKinhDoanh(int draw, int start, int length, string searchValue)
        {
            var query = (await _sanPhamChiTietService.GetDanhSachGiayNgungKinhDoanhAynsc())
                .Skip(start)
                .Take(length)
                .ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                query = (await _sanPhamChiTietService.GetDanhSachGiayNgungKinhDoanhAynsc()).Where(x =>
                x.SanPham!.ToLower().Contains(searchValueLower) ||
                x.LoaiGiay!.ToLower().Contains(searchValueLower) ||
                x.ChatLieu!.ToLower().Contains(searchValueLower) ||
                x.KieuDeGiay!.ToLower().Contains(searchValueLower)
                )
                .Skip(start)
                .Take(length)
                .ToList();
            }

            var totalRecords = (await _sanPhamChiTietService.GetDanhSachGiayNgungKinhDoanhAynsc()).Count;

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = query
            });
        }


        // GET: Admin/SanPhamChiTiet/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.sanPhamChiTiets == null)
            {
                return NotFound();
            }

            var sanPhamChiTiet = await _context.sanPhamChiTiets
                .Include(s => s.ChatLieu)
                .Include(s => s.KichCo)
                .Include(s => s.KieuDeGiay)
                .Include(s => s.LoaiGiay)
                .Include(s => s.MauSac)
                .Include(s => s.SanPham)
                .Include(s => s.ThuongHieu)
                .Include(s => s.XuatXu)
                .FirstOrDefaultAsync(m => m.IdChiTietSp == id);
            if (sanPhamChiTiet == null)
            {
                return NotFound();
            }

            return View(sanPhamChiTiet);
        }

        // GET: Admin/SanPhamChiTiet/ManageSanPham
        public IActionResult ManageSanPham()
        {
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "Ten");
            return View();
        }

        public async Task<IActionResult> LoadPartialView(string idSanPhamChiTiet)
        {
            var model = await _sanPhamChiTietService.GetSanPhamChiTietViewModelByKeyAsync(idSanPhamChiTiet);
            return PartialView("_DetailPartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckSanPhamAddOrUpdate([FromBody] SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            return Json(await _sanPhamChiTietService.CheckSanPhamAddOrUpdate(sanPhamChiTietDTO));
        }

        // POST: Admin/SanPhamChiTiet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            if (ModelState.IsValid)
            {
                return Json(await _sanPhamChiTietService.AddAysnc(sanPhamChiTietDTO));
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task CreateAnh([FromForm] string IdChiTietSp, [FromForm] List<IFormFile> lstIFormFile)
        {
            await _sanPhamChiTietService.CreateAnhAysnc(IdChiTietSp, lstIFormFile);
        }

        [HttpPost]
        public async Task<bool> UpdateSanPham([FromBody] SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            return await _sanPhamChiTietService.UpdateAynsc(sanPhamChiTietDTO);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenSanPham([FromBody] SanPhamDTO sanPhamDTO)
        {
            return Json(await _sanPhamChiTietService.CreateTenSanPhamAynsc(sanPhamDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenThuongHieu([FromBody] ThuongHieuDTO thuongHieuDTO)
        {
            return Json(await _sanPhamChiTietService.CreateTenThuongHieuAynsc(thuongHieuDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenChatLieu([FromBody] ChatLieuDTO chatLieuDTO)
        {
            return Json(await _sanPhamChiTietService.CreateTenChatLieuAynsc(chatLieuDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenLoaiGiay([FromBody] LoaiGiayDTO loaiGiayDTO)
        {
            return Json(await _sanPhamChiTietService.CreateTenLoaiGiayAynsc(loaiGiayDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenKieuDeGiay([FromBody] KieuDeGiayDTO kieuDeGiayDTO)
        {
            return Json(await _sanPhamChiTietService.CreateTenKieuDeGiayAynsc(kieuDeGiayDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenMauSac([FromBody] MauSacDTO kieuDeGiayDTO)
        {
            return Json(await _sanPhamChiTietService.CreateTenMauSacAynsc(kieuDeGiayDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenKichCo([FromBody] KichCoDTO kichCoDTO)
        {
            return Json(await _sanPhamChiTietService.CreateTenKichCoAynsc(kichCoDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenXuatXu([FromBody] XuatXuDTO xuatXuDTO)
        {
            return Json(await _sanPhamChiTietService.CreateTenXuatXuAynsc(xuatXuDTO));
        }

        [HttpPost]
        public async Task DeleteAnh([FromForm] ImageDTO responseImageDeleteVM)
        {
            await _sanPhamChiTietService.DeleteAnhAysnc(responseImageDeleteVM);
        }

        // GET: Admin/SanPhamChiTiet/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.sanPhamChiTiets == null)
            {
                return NotFound();
            }

            var sanPhamChiTiet = await _context.sanPhamChiTiets.FindAsync(id);
            if (sanPhamChiTiet == null)
            {
                return NotFound();
            }
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "IdChatLieu", sanPhamChiTiet.IdChatLieu);
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "IdKichCo", sanPhamChiTiet.IdKichCo);
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "IdKieuDeGiay", sanPhamChiTiet.IdKieuDeGiay);
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "IdLoaiGiay", sanPhamChiTiet.IdLoaiGiay);
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "IdMauSac", sanPhamChiTiet.IdMauSac);
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "IdSanPham", sanPhamChiTiet.IdSanPham);
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "IdThuongHieu", sanPhamChiTiet.IdThuongHieu);
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "IdXuatXu", sanPhamChiTiet.IdXuatXu);
            return View(sanPhamChiTiet);
        }

        // POST: Admin/SanPhamChiTiet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdChiTietSp,Ma,Day,MoTa,SoLuongTon,GiaBan,GiaNhap,TrangThai,TrangThaiSale,IdSanPham,IdKieuDeGiay,IdXuatXu,IdChatLieu,IdMauSac,IdKichCo,IdLoaiGiay,IdThuongHieu")] SanPhamChiTiet sanPhamChiTiet)
        {
            if (id != sanPhamChiTiet.IdChiTietSp)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sanPhamChiTiet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SanPhamChiTietExists(sanPhamChiTiet.IdChiTietSp))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdChatLieu"] = new SelectList(_context.ChatLieus, "IdChatLieu", "IdChatLieu", sanPhamChiTiet.IdChatLieu);
            ViewData["IdKichCo"] = new SelectList(_context.kichCos, "IdKichCo", "IdKichCo", sanPhamChiTiet.IdKichCo);
            ViewData["IdKieuDeGiay"] = new SelectList(_context.kieuDeGiays, "IdKieuDeGiay", "IdKieuDeGiay", sanPhamChiTiet.IdKieuDeGiay);
            ViewData["IdLoaiGiay"] = new SelectList(_context.LoaiGiays, "IdLoaiGiay", "IdLoaiGiay", sanPhamChiTiet.IdLoaiGiay);
            ViewData["IdMauSac"] = new SelectList(_context.mauSacs, "IdMauSac", "IdMauSac", sanPhamChiTiet.IdMauSac);
            ViewData["IdSanPham"] = new SelectList(_context.SanPhams, "IdSanPham", "IdSanPham", sanPhamChiTiet.IdSanPham);
            ViewData["IdThuongHieu"] = new SelectList(_context.thuongHieus, "IdThuongHieu", "IdThuongHieu", sanPhamChiTiet.IdThuongHieu);
            ViewData["IdXuatXu"] = new SelectList(_context.xuatXus, "IdXuatXu", "IdXuatXu", sanPhamChiTiet.IdXuatXu);
            return View(sanPhamChiTiet);
        }

        // GET: Admin/SanPhamChiTiet/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _sanPhamChiTietService.DeleteAysnc(id);
            return Ok(result);
        }

        // POST: Admin/SanPhamChiTiet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.sanPhamChiTiets == null)
            {
                return Problem("Entity set 'BazaizaiContext.sanPhamChiTiets'  is null.");
            }
            var sanPhamChiTiet = await _context.sanPhamChiTiets.FindAsync(id);
            if (sanPhamChiTiet != null)
            {
                _context.sanPhamChiTiets.Remove(sanPhamChiTiet);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SanPhamChiTietExists(string id)
        {
            return (_context.sanPhamChiTiets?.Any(e => e.IdChiTietSp == id)).GetValueOrDefault();
        }
    }
}

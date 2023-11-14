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
using System.Text;
using System.Text.Json;
using Google.Apis.PeopleService.v1.Data;
using System.Net.Http.Headers;
using Newtonsoft.Json.Serialization;
using DocumentFormat.OpenXml;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using static App_View.Areas.Admin.Controllers.SanPhamChiTietController;
using DocumentFormat.OpenXml.Bibliography;
using static App_Data.Repositories.TrangThai;
using static Google.Apis.Requests.BatchRequest;
using App_Data.ViewModels.SanPhamChiTietViewModel;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SanPhamChiTietController : Controller
    {
        private readonly ISanPhamChiTietService _sanPhamChiTietService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly BazaizaiContext _bazaizaiContext;
        private readonly HttpClient _httpClient;

        public SanPhamChiTietController(ISanPhamChiTietService sanPhamChiTietService, IWebHostEnvironment webHostEnvironment, HttpClient httpClient)
        {
            _sanPhamChiTietService = sanPhamChiTietService;
            _webHostEnvironment = webHostEnvironment;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _httpClient = httpClient;
            _bazaizaiContext = new BazaizaiContext();
        }
        [HttpGet]
        // GET: Admin/SanPhamChiTiet/DanhSachSanPham
        public async Task<IActionResult> DanhSachSanPham()
        {
            ViewData["ThuongHieu"] = new SelectList(await _sanPhamChiTietService.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["KichCo"] = new SelectList((await _sanPhamChiTietService.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["MauSac"] = new SelectList(await _sanPhamChiTietService.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["SanPham"] = new SelectList(await _sanPhamChiTietService.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
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

        public class ErrorRow
        {
            public int Row { get; set; }
            public string? SanPham { get; set; }
            public string? ThuongHieu { get; set; }
            public string? XuatXu { get; set; }
            public string? ChatLieu { get; set; }
            public string? LoaiGiay { get; set; }
            public string? KieuDeGiay { get; set; }
            public string? MauSac { get; set; }
            public string? KichCo { get; set; }
            public string? GiaNhap { get; set; }
            public string? GiaBan { get; set; }
            public string? SoLuong { get; set; }
            public string? KhoiLuong { get; set; }
            public bool? Day { get; set; }
            public bool? NoiBat { get; set; }
            public bool? TrangThaiSale { get; set; }
            public string? ListTenAnh { get; set; }

            public string? ErrorMessage { get; set; }
        }


        [HttpPost]
        public async Task<ActionResult> ImportProducts(IFormFile file)
        {
            int slSuccess = 0;
            int slFalse = 0;
            List<ErrorRow> errorRows = new List<ErrorRow>();
            var sanpham = "";
            var thuongHieu = "";
            var xuatXu = "";
            var chatLieu = "";
            var loaiGiay = "";
            var kieuDeGiay = "";
            var mauSac = "";
            var kichCo = "";
            var giaNhap = "";
            var giaBan = "";
            var soLuong = "";
            var khoiLuong = "";
            var day = "";
            var noiBat = "";
            var trangThaiSale = "";
            var listTenAnh = new List<string>();

            if (file != null && file.Length > 0)
            {
                try
                {
                    using (var stream = file.OpenReadStream())
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];

                        int rowCount = worksheet.Dimension.Rows;
                        int colCount = worksheet.Dimension.Columns;


                        for (int row = 2; row <= rowCount; row++)
                        {
                            sanpham = worksheet.Cells[row, 1].Text;
                            thuongHieu = worksheet.Cells[row, 2].Text;
                            xuatXu = worksheet.Cells[row, 3].Text;
                            chatLieu = worksheet.Cells[row, 4].Text;
                            loaiGiay = worksheet.Cells[row, 5].Text;
                            kieuDeGiay = worksheet.Cells[row, 6].Text;
                            mauSac = worksheet.Cells[row, 7].Text;
                            kichCo = worksheet.Cells[row, 8].Text;
                            giaNhap = worksheet.Cells[row, 9].Text;
                            giaBan = worksheet.Cells[row, 10].Text;
                            soLuong = worksheet.Cells[row, 11].Text;
                            khoiLuong = worksheet.Cells[row, 12].Text;
                            day = worksheet.Cells[row, 13].Text;
                            noiBat = worksheet.Cells[row, 14].Text;
                            trangThaiSale = worksheet.Cells[row, 15].Text;
                            listTenAnh = worksheet.Cells[row, 16].Text.Split(',').ToList();
                            if (
                                !string.IsNullOrEmpty(sanpham) &&
                                !string.IsNullOrEmpty(thuongHieu) &&
                                !string.IsNullOrEmpty(xuatXu) &&
                                !string.IsNullOrEmpty(chatLieu) &&
                                !string.IsNullOrEmpty(loaiGiay) &&
                                !string.IsNullOrEmpty(kieuDeGiay) &&
                                !string.IsNullOrEmpty(mauSac) &&
                                !string.IsNullOrEmpty(kichCo) &&
                                !string.IsNullOrEmpty(giaNhap) &&
                                !string.IsNullOrEmpty(giaBan) &&
                                !string.IsNullOrEmpty(soLuong) &&
                                !string.IsNullOrEmpty(khoiLuong) &&
                                !string.IsNullOrEmpty(noiBat) &&
                                !string.IsNullOrEmpty(trangThaiSale) &&
                                listTenAnh.Any()
                                )
                            {
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
                                sanPhamDTO.GiaNhap = string.IsNullOrEmpty(giaNhap) ? null : Convert.ToDouble(giaNhap);
                                sanPhamDTO.SoLuongTon = string.IsNullOrEmpty(giaNhap) ? null : Convert.ToInt32(soLuong);
                                sanPhamDTO.GiaBan = Convert.ToDouble(giaBan);
                                sanPhamDTO.KhoiLuong = Convert.ToDouble(khoiLuong);
                                sanPhamDTO.Day = day == "1" ? true : false;
                                sanPhamDTO.TrangThaiKhuyenMai = trangThaiSale == "1" ? true : false;
                                sanPhamDTO.NoiBat = noiBat == "1" ? true : false;
                                var response = (await _sanPhamChiTietService.AddAysnc(sanPhamDTO));

                                if (response.Success)
                                {
                                    slSuccess++;

                                    var formContent = new MultipartFormDataContent();
                                    formContent.Add(new StringContent(response.IdChiTietSp!), "idProductDetail");
                                    for (int i = 0; i < listTenAnh.Count(); i++)
                                    {
                                        formContent.Add(new StringContent(listTenAnh[i]), $"lstNameImage[{i}]");
                                    }
                                    try
                                    {
                                        HttpResponseMessage responseCreate = await _httpClient.PostAsync("/api/Anh/create-list-model-image", formContent);
                                        responseCreate.EnsureSuccessStatusCode();

                                    }
                                    catch (HttpRequestException ex)
                                    {
                                        slFalse++;
                                        Console.WriteLine($"Lỗi gửi yêu cầu HTTP: {ex.Message}");
                                    }
                                    catch (Exception ex)
                                    {
                                        slFalse++;
                                        Console.WriteLine($"Lỗi khác: {ex.Message}");
                                    }
                                }
                                else
                                {
                                    slFalse++;
                                    var errorRow = new ErrorRow
                                    {
                                        SanPham = sanpham,
                                        ThuongHieu = thuongHieu,
                                        XuatXu = xuatXu,
                                        ChatLieu = chatLieu,
                                        LoaiGiay = loaiGiay,
                                        KieuDeGiay = kieuDeGiay,
                                        MauSac = mauSac,
                                        KichCo = kichCo,
                                        GiaNhap = giaNhap,
                                        GiaBan = giaBan,
                                        SoLuong = soLuong,
                                        KhoiLuong = khoiLuong,
                                        Day = day == "1",
                                        NoiBat = noiBat == "1",
                                        TrangThaiSale = trangThaiSale == "1",
                                        ListTenAnh = string.Join(",", listTenAnh),
                                        ErrorMessage = response.DescriptionErr
                                    };

                                    errorRows.Add(errorRow);

                                }
                            }
                        }
                    }

                }
                catch (Exception)
                {
                    slFalse++;
                    var errorRow = new ErrorRow
                    {
                        SanPham = sanpham,
                        ThuongHieu = thuongHieu,
                        XuatXu = xuatXu,
                        ChatLieu = chatLieu,
                        LoaiGiay = loaiGiay,
                        KieuDeGiay = kieuDeGiay,
                        MauSac = mauSac,
                        KichCo = kichCo,
                        GiaNhap = giaNhap,
                        GiaBan = giaBan,
                        SoLuong = soLuong,
                        KhoiLuong = khoiLuong,
                        Day = day == "1",
                        NoiBat = noiBat == "1",
                        TrangThaiSale = trangThaiSale == "1",
                        ListTenAnh = string.Join(",", listTenAnh),
                        ErrorMessage = "Sai định dạng hoặc để trống trường"
                    };

                    errorRows.Add(errorRow);
                }
            }
            if (errorRows.Count > 0)
            {
                using (var errorPackage = new ExcelPackage())
                {
                    var errorWorksheet = errorPackage.Workbook.Worksheets.Add("Errors");
                    using (var range = errorWorksheet.Cells[1, 1, 1, 17])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                    }

                    errorWorksheet.Cells[1, 1].Value = "Tên SP";
                    errorWorksheet.Cells[1, 2].Value = "Thương Hiệu";
                    errorWorksheet.Cells[1, 3].Value = "Xuất xứ";
                    errorWorksheet.Cells[1, 4].Value = "Chất liệu";
                    errorWorksheet.Cells[1, 5].Value = "Loại giầy";
                    errorWorksheet.Cells[1, 6].Value = "Kiểu đế giầy";
                    errorWorksheet.Cells[1, 7].Value = "Màu sắc";
                    errorWorksheet.Cells[1, 8].Value = "Kích cỡ";
                    errorWorksheet.Cells[1, 9].Value = "Giá nhập";
                    errorWorksheet.Cells[1, 10].Value = "Giá bán";
                    errorWorksheet.Cells[1, 11].Value = "Số lượng";
                    errorWorksheet.Cells[1, 12].Value = "Khối lượng";
                    errorWorksheet.Cells[1, 13].Value = "Dây";
                    errorWorksheet.Cells[1, 14].Value = "Nổi bật";
                    errorWorksheet.Cells[1, 15].Value = "Trạng thái Sale";
                    errorWorksheet.Cells[1, 16].Value = "Ảnh";
                    errorWorksheet.Cells[1, 17].Value = "Mô tả lỗi";

                    // Dữ liệu
                    for (int i = 0; i < errorRows.Count; i++)
                    {
                        var errorRow = errorRows[i];

                        errorWorksheet.Cells[i + 2, 1].Value = errorRow.SanPham;
                        errorWorksheet.Cells[i + 2, 2].Value = errorRow.ThuongHieu;
                        errorWorksheet.Cells[i + 2, 3].Value = errorRow.XuatXu;
                        errorWorksheet.Cells[i + 2, 4].Value = errorRow.ChatLieu;
                        errorWorksheet.Cells[i + 2, 5].Value = errorRow.LoaiGiay;
                        errorWorksheet.Cells[i + 2, 6].Value = errorRow.KieuDeGiay;
                        errorWorksheet.Cells[i + 2, 7].Value = errorRow.MauSac;
                        errorWorksheet.Cells[i + 2, 8].Value = errorRow.KichCo;
                        errorWorksheet.Cells[i + 2, 9].Value = errorRow.GiaNhap;
                        errorWorksheet.Cells[i + 2, 10].Value = errorRow.GiaBan;
                        errorWorksheet.Cells[i + 2, 11].Value = errorRow.SoLuong;
                        errorWorksheet.Cells[i + 2, 12].Value = errorRow.KhoiLuong;
                        errorWorksheet.Cells[i + 2, 13].Value = errorRow.Day;
                        errorWorksheet.Cells[i + 2, 14].Value = errorRow.NoiBat;
                        errorWorksheet.Cells[i + 2, 15].Value = errorRow.TrangThaiSale;
                        errorWorksheet.Cells[i + 2, 16].Value = errorRow.ListTenAnh;
                        errorWorksheet.Cells[i + 2, 17].Value = errorRow.ErrorMessage;
                    }

                    var errorBytes = errorPackage.GetAsByteArray();
                    var errorFileName = "ImportErrors.xlsx";
                    var errorFilePath = Path.Combine(_webHostEnvironment.WebRootPath, errorFileName);
                    if (System.IO.File.Exists(errorFilePath))
                    {
                        System.IO.File.Delete(errorFilePath);
                    }
                    System.IO.File.WriteAllBytes(errorFilePath, errorBytes);
                    return Ok(new { Success = false, ErrorFilePath = errorFilePath, slFalse, slSuccess });
                }
            }


            return Ok(new { slFalse, slSuccess });

        }

        #endregion


        #region DownLoadFile
        public IActionResult DownloadFileTemplate()
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

        public IActionResult DownloadFileErr()
        {
            string relativePath = "ImportErrors.xlsx";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);
            string fileName = "ImportErrors.xlsx";

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
            ViewData["IdChatLieu"] = new SelectList(await _sanPhamChiTietService.GetListModelChatLieuAsync(), "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList((await _sanPhamChiTietService.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(await _sanPhamChiTietService.GetListModelKieuDeGiayAsync(), "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(await _sanPhamChiTietService.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(await _sanPhamChiTietService.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(await _sanPhamChiTietService.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(await _sanPhamChiTietService.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(await _sanPhamChiTietService.GetListModelXuatXuAsync(), "IdXuatXu", "Ten");
            var model = await _sanPhamChiTietService.GetListSanPhamChiTietDTOAsync(listGuildDTO);
            return PartialView("_DanhSachSanPhamUpdate", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartialViewSanPhamCopy(string IdSanPhamChiTiet)
        {
            ViewData["IdChatLieu"] = new SelectList(await _sanPhamChiTietService.GetListModelChatLieuAsync(), "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList((await _sanPhamChiTietService.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(await _sanPhamChiTietService.GetListModelKieuDeGiayAsync(), "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(await _sanPhamChiTietService.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(await _sanPhamChiTietService.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(await _sanPhamChiTietService.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(await _sanPhamChiTietService.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(await _sanPhamChiTietService.GetListModelXuatXuAsync(), "IdXuatXu", "Ten");

            var model = (await _sanPhamChiTietService
                .GetListSanPhamChiTietDTOAsync(new ListGuildDTO()
                {
                    listGuild = new List<string>()
                {
                    IdSanPhamChiTiet
                }
                })
                )
            .FirstOrDefault();

            return PartialView("_SanPhamCopyPartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSanPhamChiTietCoppy([FromForm] SanPhamChiTietCopyDTO sanPhamChiTietCopyDTO)
        {
            var multipartContent = new MultipartFormDataContent();

            if (sanPhamChiTietCopyDTO.ListAnhCreate != null && sanPhamChiTietCopyDTO.ListAnhCreate.Any())
            {
                foreach (var file in sanPhamChiTietCopyDTO.ListAnhCreate)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = $"SanPhamChiTietCopyDTO.ListAnhCreate",
                        FileName = file.FileName
                    };

                    multipartContent.Add(fileContent, $"SanPhamChiTietCopyDTO.ListAnhCreate");
                }
            }

            if (sanPhamChiTietCopyDTO.ListTenAnhRemove != null && sanPhamChiTietCopyDTO.ListTenAnhRemove.Any())
            {
                foreach (var ten in sanPhamChiTietCopyDTO.ListTenAnhRemove)
                {
                    multipartContent.Add(new StringContent(ten), $"SanPhamChiTietCopyDTO.ListTenAnhRemove");
                }
            }



            var sanPhamChiTietDataProperties = typeof(SanPhamChiTietDTO).GetProperties();
            foreach (var property in sanPhamChiTietDataProperties)
            {
                var value = property.GetValue(sanPhamChiTietCopyDTO.SanPhamChiTietData);

                if (!(value is List<string>) && !(value is List<int>) && !(value is List<IFormFile>))
                {
                    var stringValue = value?.ToString() ?? string.Empty;
                    multipartContent.Add(new StringContent(stringValue), $"SanPhamChiTietCopyDTO.SanPhamChiTietData.{property.Name}");
                }

                if (property.Name == "DanhSachAnh")
                {
                    foreach (var item in sanPhamChiTietCopyDTO.SanPhamChiTietData!.DanhSachAnh!)
                    {
                        multipartContent.Add(new StringContent(item), $"SanPhamChiTietCopyDTO.SanPhamChiTietData.DanhSachAnh");
                    }
                }
            }

            var response = await _httpClient.PostAsync("/api/SanPhamChiTiet/Creat-SanPhamChiTietCopy", multipartContent);

            return Ok(await response.Content.ReadAsAsync<bool>());
        }

        public async Task<IActionResult> GetDanhSachSanPham([FromBody] FilterAdminDTO filterAdminDTO)
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
                query = query.Where(x => x.SanPham!.ToLower().Contains(filterAdminDTO.SanPham.ToLower()))
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

        // GET: Admin/SanPhamChiTiet/ManageSanPham
        public async Task<IActionResult> ManageSanPham()
        {
            ViewData["IdChatLieu"] = new SelectList(await _sanPhamChiTietService.GetListModelChatLieuAsync(), "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList((await _sanPhamChiTietService.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(await _sanPhamChiTietService.GetListModelKieuDeGiayAsync(), "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(await _sanPhamChiTietService.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(await _sanPhamChiTietService.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(await _sanPhamChiTietService.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(await _sanPhamChiTietService.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(await _sanPhamChiTietService.GetListModelXuatXuAsync(), "IdXuatXu", "Ten");
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

        // GET: Admin/SanPhamChiTiet/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _sanPhamChiTietService.DeleteAysnc(id);
            return Ok(result);
        }

        public IActionResult TongQuanSanPham()
        {
            return View();
        }

        public IActionResult DanhSachTongQuanSanPham()
        {
            return PartialView();
        }

        public class SPDanhSachViewModel
        {
            public string? SumGuild { get; set; }
            public string? SanPham { get; set; }
            public string? ThuongHieu { get; set; }
            public string? LoaiGiay { get; set; }
            public string? KieuDeGiay { get; set; }
            public string? XuatXu { get; set; }
            public string? ChatLieu { get; set; }
            public int SoMau { get; set; }
            public int SoSize { get; set; }
            public int TongSoLuongTon { get; set; }
            public double TongSoLuongDaBan { get; set; }
        }

        public async Task<IActionResult> GetRelatedProducts(string sumGuid)
        {
            var listGuid = sumGuid.Split('/');
            var idSanPham = listGuid[0];
            var idThuongHieu = listGuid[1];
            var idKieuDeGiay = listGuid[2];
            var idLoaiGiay = listGuid[3];
            var idXuatXu = listGuid[4];
            var idChatLieu = listGuid[5];
            var lstRelatedProducts = await _bazaizaiContext
                .sanPhamChiTiets
                .Where(sp =>
                sp.IdSanPham == idSanPham &&
                sp.IdThuongHieu == idThuongHieu &&
                sp.IdKieuDeGiay == idKieuDeGiay &&
                sp.IdLoaiGiay == idLoaiGiay &&
                sp.IdXuatXu == idXuatXu &&
                sp.IdChatLieu == idChatLieu
                ).
                Include(sp => sp.SanPham).
                Include(sp => sp.MauSac).
                Include(sp => sp.KichCo).
                Include(sp => sp.Anh).
                Select(sp => new RelatedProductViewModel()
                {
                    IdSanPham = sp.IdSanPham,
                    MaSanPham = sp.Ma,
                    Anh = sp.Anh.OrderBy(a=>a.NgayTao).FirstOrDefault()!.Url,
                    MauSac = sp.MauSac.TenMauSac,
                    GiaBan = sp.GiaBan.GetValueOrDefault(),
                    KichCo = sp.KichCo.SoKichCo.GetValueOrDefault(),
                    SanPham = sp.SanPham.TenSanPham,
                    SoLuong = sp.SoLuongTon.GetValueOrDefault(),
                    SoLuongDaBan = sp.SoLuongDaBan.GetValueOrDefault(),
                    //TGBanGanDay = 
                })
                .ToListAsync();
            return PartialView(lstRelatedProducts);
        }

        public async Task<IActionResult> GetTongQuanDanhSach(int draw, int start, int length, string searchValue)
        {
            var danhSanSanPham = await _bazaizaiContext
                .sanPhamChiTiets
                .Include(sp => sp.SanPham)
                .Include(sp => sp.ThuongHieu)
                .Include(sp => sp.KieuDeGiay)
                .Include(sp => sp.LoaiGiay)
                .Include(sp => sp.XuatXu)
                .Include(sp => sp.ChatLieu)
                .GroupBy(gr => new
                {
                    gr.IdChatLieu,
                    gr.IdSanPham,
                    gr.IdThuongHieu,
                    gr.IdXuatXu,
                    gr.IdLoaiGiay,
                    gr.IdKieuDeGiay,
                })
                .Select(gr => new SPDanhSachViewModel()
                {
                    SumGuild = $"{gr.FirstOrDefault()!.IdSanPham}/{gr.FirstOrDefault()!.IdThuongHieu}/{gr.FirstOrDefault()!.IdKieuDeGiay}/{gr.FirstOrDefault()!.IdLoaiGiay}/{gr.FirstOrDefault()!.IdXuatXu}/{gr.FirstOrDefault()!.IdChatLieu}",
                    SanPham = $"{gr.FirstOrDefault()!.SanPham.TenSanPham}",
                    ChatLieu = gr.FirstOrDefault()!.ChatLieu.TenChatLieu,
                    KieuDeGiay = gr.FirstOrDefault()!.KieuDeGiay.TenKieuDeGiay,
                    LoaiGiay = gr.FirstOrDefault()!.LoaiGiay.TenLoaiGiay,
                    ThuongHieu = gr.FirstOrDefault()!.ThuongHieu.TenThuongHieu,
                    XuatXu = gr.FirstOrDefault()!.XuatXu.Ten,
                    SoMau = gr.Select(it => it.IdMauSac).Distinct().Count(),
                    SoSize = gr.Select(it => it.IdKichCo).Distinct().Count(),
                    TongSoLuongTon = gr.Sum(it => it.SoLuongTon.GetValueOrDefault()),
                    TongSoLuongDaBan = gr.Sum(it => it.SoLuongDaBan.GetValueOrDefault())
                })
                .ToListAsync();

            var query = (danhSanSanPham)
                .Skip(start)
                .Take(length)
                .ToList();

            var totalRecords = (danhSanSanPham).Count;

            return Json(new
            {
                draw = draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords,
                data = query
            });
        }
    }
}

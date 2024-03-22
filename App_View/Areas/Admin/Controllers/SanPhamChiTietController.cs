using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App_Data.DbContext;
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
using App_Data.ViewModels.FilterViewModel;
using Microsoft.DotNet.MSIdentity.Shared;
using DocumentFormat.OpenXml.Spreadsheet;
using App_Data.ViewModels.FilterDTO;
using Microsoft.AspNetCore.Authorization;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SanPhamChiTietController : Controller
    {
        private readonly ISanPhamChiTietservice _SanPhamChiTietservice;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpClient _httpClient;

        public SanPhamChiTietController(ISanPhamChiTietservice SanPhamChiTietservice, IWebHostEnvironment webHostEnvironment, HttpClient httpClient)
        {
            _SanPhamChiTietservice = SanPhamChiTietservice;
            _webHostEnvironment = webHostEnvironment;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            _httpClient = httpClient;
        }
        [HttpGet]
        // GET: Admin/SanPhamChiTiet/DanhSachSanPham
        public async Task<IActionResult> DanhSachSanPham()
        {
            ViewData["ThuongHieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["KichCo"] = new SelectList((await _SanPhamChiTietservice.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["MauSac"] = new SelectList(await _SanPhamChiTietservice.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["SanPham"] = new SelectList(await _SanPhamChiTietservice.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["LoaiGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
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
            return Ok(await _SanPhamChiTietservice.NgungKinhDoanhSanPhamAynsc(listGuildDTO));
        }

        [HttpPost]
        public async Task<IActionResult> KinhDoanhLaiListSanPham([FromBody] ListGuildDTO listGuildDTO)
        {
            return Ok(await _SanPhamChiTietservice.KinhDoanhLaiSanPhamAynsc(listGuildDTO));
        }

        [HttpGet]
        public async Task<IActionResult> KhoiPhucKinhDoanh(string id)
        {
            return Ok(await _SanPhamChiTietservice.KhoiPhucKinhDoanhAynsc(id));
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
            public string? MoTa { get; set; }
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
            var moTa = "";
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
                        Console.WriteLine(rowCount);
                        Console.WriteLine(colCount);

                        for (int row = 2; row <= rowCount; row++)
                        {
                            bool isRowEmpty = true;
                            Console.WriteLine(row);
                            Console.WriteLine(rowCount);
                            for (int col = 1; col <= colCount; col++)
                            {
                                Console.WriteLine(row.ToString() + "-" + worksheet.Cells[row, col].Text + "-");
                                if (!string.IsNullOrWhiteSpace(worksheet.Cells[row, col].Text))
                                {
                                    isRowEmpty = false;
                                    break;
                                }
                            }

                            if (isRowEmpty)
                            {
                                break;
                            }

                            sanpham = worksheet.Cells[row, 1].Text;
                            thuongHieu = worksheet.Cells[row, 2].Text;
                            xuatXu = worksheet.Cells[row, 3].Text;
                            chatLieu = worksheet.Cells[row, 4].Text;
                            loaiGiay = worksheet.Cells[row, 5].Text;
                            kieuDeGiay = worksheet.Cells[row, 6].Text;
                            mauSac = worksheet.Cells[row, 7].Text;
                            kichCo = worksheet.Cells[row, 8].Text;
                            giaNhap = worksheet.Cells[row, 9].Text.Replace(",", "").Replace("₫", "");
                            giaBan = worksheet.Cells[row, 10].Text.Replace(",", "").Replace("₫", "");
                            soLuong = worksheet.Cells[row, 11].Text;
                            khoiLuong = worksheet.Cells[row, 12].Text;
                            day = worksheet.Cells[row, 13].Text;
                            noiBat = worksheet.Cells[row, 14].Text;
                            trangThaiSale = worksheet.Cells[row, 15].Text;
                            moTa = worksheet.Cells[row, 16].Text;
                            listTenAnh = !string.IsNullOrWhiteSpace(worksheet.Cells[row, 17].Text) ? worksheet.Cells[row, 17].Text.Split(',').ToList() : new List<string>();
                            Console.WriteLine(sanpham + "-" + mauSac + "-" + kichCo);
                            Console.WriteLine();

                            if (
                                !string.IsNullOrEmpty(sanpham) &&
                                !string.IsNullOrEmpty(thuongHieu) &&
                                !string.IsNullOrEmpty(xuatXu) &&
                                !string.IsNullOrEmpty(chatLieu) &&
                                !string.IsNullOrEmpty(loaiGiay) &&
                                !string.IsNullOrEmpty(kieuDeGiay) &&
                                !string.IsNullOrEmpty(mauSac) &&
                                mauSac.Length > 2 &&
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
                                var sanPhamDTO = await _SanPhamChiTietservice.GetItemExcelAynsc(new BienTheDTO
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

                                if (sanPhamDTO == null)
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
                                        MoTa = moTa,
                                        ListTenAnh = string.Join(",", listTenAnh),
                                        ErrorMessage = "Đầu vào không hợp lệ"
                                    };

                                    errorRows.Add(errorRow);
                                }
                                else
                                {
                                    try
                                    {

                                        sanPhamDTO!.GiaNhap = string.IsNullOrEmpty(giaNhap) ? null : Convert.ToDouble(giaNhap);
                                        sanPhamDTO.SoLuongTon = string.IsNullOrEmpty(giaNhap) ? null : Convert.ToInt32(soLuong);
                                        sanPhamDTO.GiaBan = Convert.ToDouble(giaBan);
                                        sanPhamDTO.KhoiLuong = Convert.ToDouble(khoiLuong);
                                        sanPhamDTO.Day = day == "1" ? true : false;
                                        sanPhamDTO.TrangThaiKhuyenMai = trangThaiSale == "1" ? true : false;
                                        sanPhamDTO.NoiBat = noiBat == "1" ? true : false;
                                        sanPhamDTO.MoTa = moTa;

                                        if (sanPhamDTO!.GiaNhap < 0 || sanPhamDTO.SoLuongTon < 0 || sanPhamDTO.GiaBan < 0 || sanPhamDTO.KhoiLuong < 0)
                                        {
                                            throw new Exception();
                                        }

                                        var response = (await _SanPhamChiTietservice.AddAysnc(sanPhamDTO));

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
                                                MoTa = moTa,
                                                ListTenAnh = string.Join(",", listTenAnh),
                                                ErrorMessage = response.DescriptionErr
                                            };

                                            errorRows.Add(errorRow);

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
                                            MoTa = moTa,
                                            ListTenAnh = string.Join(",", listTenAnh),
                                            ErrorMessage = "Trường có đầu vào không hợp lệ"
                                        };

                                        errorRows.Add(errorRow);
                                    }
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
                                    MoTa = moTa,
                                    ListTenAnh = string.Join(",", listTenAnh),
                                    ErrorMessage = "Các trường không được để trống/Độ dài không hợp lệ."
                                };

                                errorRows.Add(errorRow);
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
                        MoTa = moTa,
                        ListTenAnh = string.Join(",", listTenAnh),
                        ErrorMessage = "Trường có đầu vào không hợp lệ"
                    };

                    errorRows.Add(errorRow);
                }
            }
            if (errorRows.Count > 0)
            {
                using (var errorPackage = new ExcelPackage())
                {
                    var errorWorksheet = errorPackage.Workbook.Worksheets.Add("Errors");
                    using (var range = errorWorksheet.Cells[1, 1, 1, 19])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        range.Style.Font.Size = 12;
                    }

                    errorWorksheet.Cells[1, 1].Value = "Tên SP*";
                    errorWorksheet.Cells[1, 2].Value = "Thương Hiệu*";
                    errorWorksheet.Cells[1, 3].Value = "Xuất xứ*";
                    errorWorksheet.Cells[1, 4].Value = "Chất liệu*";
                    errorWorksheet.Cells[1, 5].Value = "Loại giầy*";
                    errorWorksheet.Cells[1, 6].Value = "Kiểu đế giầy*";
                    errorWorksheet.Cells[1, 7].Value = "Màu sắc*";
                    errorWorksheet.Cells[1, 8].Value = "Kích cỡ*";
                    errorWorksheet.Cells[1, 9].Value = "Giá nhập*";
                    errorWorksheet.Cells[1, 10].Value = "Giá bán*";
                    errorWorksheet.Cells[1, 11].Value = "Số lượng*";
                    errorWorksheet.Cells[1, 12].Value = "Khối lượng*";
                    errorWorksheet.Cells[1, 13].Value = "Dây*";
                    errorWorksheet.Cells[1, 14].Value = "Nổi bật*";
                    errorWorksheet.Cells[1, 15].Value = "Được áp dụng khuyển mại*";
                    errorWorksheet.Cells[1, 16].Value = "Mô tả";
                    errorWorksheet.Cells[1, 17].Value = "Ảnh*";
                    errorWorksheet.Cells[1, 18].Value = "Mô tả lỗi";

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
                        errorWorksheet.Cells[i + 2, 16].Value = errorRow.MoTa;
                        errorWorksheet.Cells[i + 2, 17].Value = errorRow.ListTenAnh;
                        errorWorksheet.Cells[i + 2, 18].Value = errorRow.ErrorMessage;
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
        [HttpPost]
        public async Task<IActionResult> ExportListProductRelateFromListGuild([FromBody] ListGuildDTO listGuildDTO)
        {
            var listSanPhamChiTietViewModel = new List<SanPhamChiTietViewModel>();
            foreach (var guild in listGuildDTO.listGuild!)
            {
                listSanPhamChiTietViewModel.Add((await _SanPhamChiTietservice.GetSanPhamChiTietViewModelByKeyAsync(guild))!);
            }
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ProductList");

                using (var range = worksheet.Cells[1, 1, 1, 10])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.Font.Size = 12;
                }

                worksheet.Cells[1, 1].Value = "MSP";
                worksheet.Cells[1, 2].Value = "Sản phẩm";
                worksheet.Cells[1, 3].Value = "Màu";
                worksheet.Cells[1, 4].Value = "K/Cỡ";
                worksheet.Cells[1, 5].Value = "G/Nhập";
                worksheet.Cells[1, 6].Value = "G/Bán";
                worksheet.Cells[1, 7].Value = "Số lượng";
                worksheet.Cells[1, 8].Value = "Khối lượng";
                worksheet.Cells[1, 9].Value = "Số lượng đã bán";
                worksheet.Cells[1, 10].Value = "Kinh doanh";

                for (int i = 0; i < listSanPhamChiTietViewModel.Count(); i++)
                {
                    worksheet.Cells[i + 2, 1].Value = listSanPhamChiTietViewModel[i].Ma;
                    worksheet.Cells[i + 2, 2].Value = listSanPhamChiTietViewModel[i].SanPham;
                    worksheet.Cells[i + 2, 3].Value = listSanPhamChiTietViewModel[i].MauSac;
                    worksheet.Cells[i + 2, 4].Value = listSanPhamChiTietViewModel[i].KichCo;
                    worksheet.Cells[i + 2, 5].Value = listSanPhamChiTietViewModel[i].GiaNhap;
                    worksheet.Cells[i + 2, 6].Value = listSanPhamChiTietViewModel[i].GiaBan;
                    worksheet.Cells[i + 2, 7].Value = listSanPhamChiTietViewModel[i].SoLuongTon;
                    worksheet.Cells[i + 2, 8].Value = listSanPhamChiTietViewModel[i].KhoiLuong;
                    worksheet.Cells[i + 2, 9].Value = listSanPhamChiTietViewModel[i].SoLuongDaBan;
                    worksheet.Cells[i + 2, 10].Value = listSanPhamChiTietViewModel[i].TrangThai == 0 ? "Đang kinh doanh" : "Ngừng kinh doanh";
                }

                byte[] excelBytes = package.GetAsByteArray();

                string fileName = $"ProductList_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


        [HttpPost]
        public async Task<IActionResult> ExportSanPhamDangKinhDoanhFromListGuid([FromBody] ListGuildDTO listGuildDTO)
        {
            var lstProduct = (await _SanPhamChiTietservice.GetListSanPhamExcelAynsc()).Where(sp => listGuildDTO.listGuild!.Contains(sp.IdChiTietSp!)).ToList();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ProductList");

                using (var range = worksheet.Cells[1, 1, 1, 20])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.Font.Size = 12;
                }

                worksheet.Cells[1, 1].Value = "Mã sản phẩm";
                worksheet.Cells[1, 2].Value = "Tên sản phẩm";
                worksheet.Cells[1, 3].Value = "Thương hiệu";
                worksheet.Cells[1, 4].Value = "Kiểu đế";
                worksheet.Cells[1, 5].Value = "Chất liệu";
                worksheet.Cells[1, 6].Value = "Thể loại";
                worksheet.Cells[1, 7].Value = "Xuất xứ";
                worksheet.Cells[1, 8].Value = "Size";
                worksheet.Cells[1, 9].Value = "Màu sắc";
                worksheet.Cells[1, 10].Value = "Giá nhập";
                worksheet.Cells[1, 11].Value = "Giá bán";
                worksheet.Cells[1, 12].Value = "Dây(True/False)";
                worksheet.Cells[1, 13].Value = "Được áp dụng khuyến mại(True/False)";
                worksheet.Cells[1, 14].Value = "Hiển thị ở danh sách nổi bật(True/False)";
                worksheet.Cells[1, 15].Value = "Số lượng";
                worksheet.Cells[1, 16].Value = "Số lượng đã bán";
                worksheet.Cells[1, 17].Value = "Khối lượng(g)";
                worksheet.Cells[1, 18].Value = "Ngày tạo";
                worksheet.Cells[1, 19].Value = "Mô tả";
                worksheet.Cells[1, 20].Value = "Danh sách ảnh";

                for (int i = 0; i < lstProduct.Count(); i++)
                {
                    worksheet.Cells[i + 2, 1].Value = lstProduct[i].Ma;
                    worksheet.Cells[i + 2, 2].Value = lstProduct[i].SanPham;
                    worksheet.Cells[i + 2, 3].Value = lstProduct[i].ThuongHieu;
                    worksheet.Cells[i + 2, 4].Value = lstProduct[i].KieuDeGiay;
                    worksheet.Cells[i + 2, 5].Value = lstProduct[i].ChatLieu;
                    worksheet.Cells[i + 2, 6].Value = lstProduct[i].LoaiGiay;
                    worksheet.Cells[i + 2, 7].Value = lstProduct[i].XuatXu;
                    worksheet.Cells[i + 2, 8].Value = lstProduct[i].KichCo;
                    worksheet.Cells[i + 2, 9].Value = lstProduct[i].MauSac;
                    worksheet.Cells[i + 2, 10].Value = lstProduct[i].GiaNhap;
                    worksheet.Cells[i + 2, 11].Value = lstProduct[i].GiaBan;
                    worksheet.Cells[i + 2, 12].Value = lstProduct[i].Day;
                    worksheet.Cells[i + 2, 13].Value = lstProduct[i].TrangThaiKhuyenMai;
                    worksheet.Cells[i + 2, 14].Value = lstProduct[i].NoiBat;
                    worksheet.Cells[i + 2, 15].Value = lstProduct[i].SoLuongTon;
                    worksheet.Cells[i + 2, 16].Value = lstProduct[i].SoLuongDaBan;
                    worksheet.Cells[i + 2, 17].Value = lstProduct[i].KhoiLuong;
                    worksheet.Cells[i + 2, 18].Value = lstProduct[i].NgayTao;
                    worksheet.Cells[i + 2, 19].Value = lstProduct[i].MoTa;
                    worksheet.Cells[i + 2, 20].Value = lstProduct[i].DanhSachAnh;
                }

                byte[] excelBytes = package.GetAsByteArray();

                string fileName = $"ProductList_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


        public async Task<IActionResult> ExportToExcel()
        {
            var lstProduct = await _SanPhamChiTietservice.GetListSanPhamExcelAynsc();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ProductList");

                using (var range = worksheet.Cells[1, 1, 1, 20])
                {
                    range.Style.Font.Bold = true;
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    range.Style.Font.Size = 12;
                }

                worksheet.Cells[1, 1].Value = "Mã sản phẩm";
                worksheet.Cells[1, 2].Value = "Tên sản phẩm";
                worksheet.Cells[1, 3].Value = "Thương hiệu";
                worksheet.Cells[1, 4].Value = "Kiểu đế";
                worksheet.Cells[1, 5].Value = "Chất liệu";
                worksheet.Cells[1, 6].Value = "Thể loại";
                worksheet.Cells[1, 7].Value = "Xuất xứ";
                worksheet.Cells[1, 8].Value = "Size";
                worksheet.Cells[1, 9].Value = "Màu sắc";
                worksheet.Cells[1, 10].Value = "Giá nhập";
                worksheet.Cells[1, 11].Value = "Giá bán";
                worksheet.Cells[1, 12].Value = "Dây(True/False)";
                worksheet.Cells[1, 13].Value = "Được áp dụng khuyến mại(True/False)";
                worksheet.Cells[1, 14].Value = "Hiển thị ở danh sách nổi bật(True/False)";
                worksheet.Cells[1, 15].Value = "Số lượng";
                worksheet.Cells[1, 16].Value = "Số lượng đã bán";
                worksheet.Cells[1, 17].Value = "Khối lượng(g)";
                worksheet.Cells[1, 18].Value = "Ngày tạo";
                worksheet.Cells[1, 19].Value = "Mô tả";
                worksheet.Cells[1, 20].Value = "Danh sách ảnh";

                for (int i = 0; i < lstProduct.Count(); i++)
                {
                    worksheet.Cells[i + 2, 1].Value = lstProduct[i].Ma;
                    worksheet.Cells[i + 2, 2].Value = lstProduct[i].SanPham;
                    worksheet.Cells[i + 2, 3].Value = lstProduct[i].ThuongHieu;
                    worksheet.Cells[i + 2, 4].Value = lstProduct[i].KieuDeGiay;
                    worksheet.Cells[i + 2, 5].Value = lstProduct[i].ChatLieu;
                    worksheet.Cells[i + 2, 6].Value = lstProduct[i].LoaiGiay;
                    worksheet.Cells[i + 2, 7].Value = lstProduct[i].XuatXu;
                    worksheet.Cells[i + 2, 8].Value = lstProduct[i].KichCo;
                    worksheet.Cells[i + 2, 9].Value = lstProduct[i].MauSac;
                    worksheet.Cells[i + 2, 10].Value = lstProduct[i].GiaNhap;
                    worksheet.Cells[i + 2, 11].Value = lstProduct[i].GiaBan;
                    worksheet.Cells[i + 2, 12].Value = lstProduct[i].Day;
                    worksheet.Cells[i + 2, 13].Value = lstProduct[i].TrangThaiKhuyenMai;
                    worksheet.Cells[i + 2, 14].Value = lstProduct[i].NoiBat;
                    worksheet.Cells[i + 2, 15].Value = lstProduct[i].SoLuongTon;
                    worksheet.Cells[i + 2, 16].Value = lstProduct[i].SoLuongDaBan;
                    worksheet.Cells[i + 2, 17].Value = lstProduct[i].KhoiLuong;
                    worksheet.Cells[i + 2, 18].Value = lstProduct[i].NgayTao;
                    worksheet.Cells[i + 2, 19].Value = lstProduct[i].MoTa;
                    worksheet.Cells[i + 2, 20].Value = lstProduct[i].DanhSachAnh;
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
            ViewData["IdChatLieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelChatLieuAsync(), "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList((await _SanPhamChiTietservice.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelKieuDeGiayAsync(), "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(await _SanPhamChiTietservice.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(await _SanPhamChiTietservice.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(await _SanPhamChiTietservice.GetListModelXuatXuAsync(), "IdXuatXu", "Ten");
            var model = await _SanPhamChiTietservice.GetListSanPhamChiTietDTOAsync(listGuildDTO);
            return PartialView("_DanhSachSanPhamUpdate", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetPartialViewSanPhamCopy(string IdSanPhamChiTiet)
        {
            ViewData["IdChatLieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelChatLieuAsync(), "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList((await _SanPhamChiTietservice.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelKieuDeGiayAsync(), "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(await _SanPhamChiTietservice.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(await _SanPhamChiTietservice.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(await _SanPhamChiTietservice.GetListModelXuatXuAsync(), "IdXuatXu", "Ten");

            var model = (await _SanPhamChiTietservice
                .GetListSanPhamChiTietDTOAsync(new ListGuildDTO()
                {
                    listGuild = new List<string>(){
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
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/sanphamchitiet/get-danh-sach-san-pham-dang-kinh-doanh", filterAdminDTO);
                if (response.IsSuccessStatusCode)
                {
                    return Content(await response.Content.ReadAsStringAsync(), "appliaction/json");
                }
                return StatusCode((int)response.StatusCode, response.ReasonPhrase);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetDanhSachSanPhamNgungKinhDoanh(int draw, int start, int length, string searchValue)
        {
            var query = (await _SanPhamChiTietservice.GetDanhSachGiayNgungKinhDoanhAynsc())
                .Skip(start)
                .Take(length)
                .ToList();

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchValueLower = searchValue.ToLower();
                query = (await _SanPhamChiTietservice.GetDanhSachGiayNgungKinhDoanhAynsc()).Where(x =>
                x.SanPham!.ToLower().Contains(searchValueLower) ||
                x.LoaiGiay!.ToLower().Contains(searchValueLower) ||
                x.ChatLieu!.ToLower().Contains(searchValueLower) ||
                x.KieuDeGiay!.ToLower().Contains(searchValueLower)
                )
                .Skip(start)
                .Take(length)
                .ToList();
            }

            var totalRecords = (await _SanPhamChiTietservice.GetDanhSachGiayNgungKinhDoanhAynsc()).Count;

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
            ViewData["IdChatLieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelChatLieuAsync(), "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList((await _SanPhamChiTietservice.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelKieuDeGiayAsync(), "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(await _SanPhamChiTietservice.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(await _SanPhamChiTietservice.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(await _SanPhamChiTietservice.GetListModelXuatXuAsync(), "IdXuatXu", "Ten");
            return View();
        }

        public async Task<IActionResult> LoadPartialView(string idSanPhamChiTiet)
        {
            var model = await _SanPhamChiTietservice.GetSanPhamChiTietViewModelByKeyAsync(idSanPhamChiTiet);
            return PartialView("_DetailPartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckSanPhamAddOrUpdate([FromBody] SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            return Json(await _SanPhamChiTietservice.CheckSanPhamAddOrUpdate(sanPhamChiTietDTO));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            if (ModelState.IsValid)
            {
                return Json(await _SanPhamChiTietservice.AddAysnc(sanPhamChiTietDTO));
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task CreateAnh([FromForm] string IdChiTietSp, [FromForm] List<IFormFile> lstIFormFile)
        {
            await _SanPhamChiTietservice.CreateAnhAysnc(IdChiTietSp, lstIFormFile);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSanPham([FromBody] SanPhamChiTietDTO sanPhamChiTietDTO)
        {
            return Ok(await _SanPhamChiTietservice.UpdateAynsc(sanPhamChiTietDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenSanPham([FromBody] SanPhamDTO sanPhamDTO)
        {
            return Json(await _SanPhamChiTietservice.CreateTenSanPhamAynsc(sanPhamDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenThuongHieu([FromBody] ThuongHieuDTO thuongHieuDTO)
        {
            return Json(await _SanPhamChiTietservice.CreateTenThuongHieuAynsc(thuongHieuDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenChatLieu([FromBody] ChatLieuDTO chatLieuDTO)
        {
            return Json(await _SanPhamChiTietservice.CreateTenChatLieuAynsc(chatLieuDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenLoaiGiay([FromBody] LoaiGiayDTO loaiGiayDTO)
        {
            return Json(await _SanPhamChiTietservice.CreateTenLoaiGiayAynsc(loaiGiayDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenKieuDeGiay([FromBody] KieuDeGiayDTO kieuDeGiayDTO)
        {
            return Json(await _SanPhamChiTietservice.CreateTenKieuDeGiayAynsc(kieuDeGiayDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenMauSac([FromBody] MauSacDTO kieuDeGiayDTO)
        {
            return Json(await _SanPhamChiTietservice.CreateTenMauSacAynsc(kieuDeGiayDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenKichCo([FromBody] KichCoDTO kichCoDTO)
        {
            return Json(await _SanPhamChiTietservice.CreateTenKichCoAynsc(kichCoDTO));
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenXuatXu([FromBody] XuatXuDTO xuatXuDTO)
        {
            return Json(await _SanPhamChiTietservice.CreateTenXuatXuAynsc(xuatXuDTO));
        }

        [HttpPost]
        public async Task DeleteAnh([FromForm] ImageDTO responseImageDeleteVM)
        {
            await _SanPhamChiTietservice.DeleteAnhAysnc(responseImageDeleteVM);
        }

        // GET: Admin/SanPhamChiTiet/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _SanPhamChiTietservice.DeleteAysnc(id);
            return Ok(result);
        }

        public IActionResult TongQuanSanPham()
        {
            return View();
        }

        public async Task<IActionResult> DanhSachTongQuanSanPham()
        {
            ViewData["IdChatLieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelChatLieuAsync(), "IdChatLieu", "TenChatLieu");
            ViewData["IdKieuDeGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelKieuDeGiayAsync(), "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdSanPham"] = new SelectList(await _SanPhamChiTietservice.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(await _SanPhamChiTietservice.GetListModelXuatXuAsync(), "IdXuatXu", "Ten");
            return PartialView();
        }


        public class SanPhamListDTO
        {
            public List<SanPhamTableDTO>? SanPhamTablesDTOs { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLstSanPhamTable([FromBody] SanPhamListDTO sanPhamListDTO)
        {
            var response = await _httpClient.PutAsJsonAsync("/api/SanPhamChiTiet/Update-List-SanPhamTable", sanPhamListDTO.SanPhamTablesDTOs);
            return Ok(await response.Content.ReadAsAsync<bool>());
        }

        public async Task<IActionResult> GetRelatedProducts(string sumGuid, string? mauSac, string? size)
        {
            var lstRelatedProducts = await _httpClient.GetFromJsonAsync<List<RelatedProductViewModel>>($"/api/SanPhamChiTiet/Get-List-RelatedProduct?sumGuild={sumGuid}");
            ViewData["MauSac"] = new SelectList(lstRelatedProducts!.Select(x => x.MauSac).Distinct());
            ViewData["Size"] = new SelectList(lstRelatedProducts!.Select(x => x.KichCo.ToString()).Distinct());
            if (!string.IsNullOrEmpty(mauSac))
            {
                ViewData["ValueMauSac"] = mauSac;
                lstRelatedProducts = lstRelatedProducts!.Where(it => it.MauSac!.ToLower() == mauSac.ToLower()).ToList();
            }
            if (!string.IsNullOrEmpty(size))
            {
                ViewData["ValueSize"] = size;
                lstRelatedProducts = lstRelatedProducts!.Where(it => it.KichCo == Convert.ToInt32(size)).ToList();
            }
            return PartialView(lstRelatedProducts!);
        }

        [HttpPost]
        public async Task<IActionResult> GetTongQuanDanhSach([FromBody] ParametersTongQuanDanhSach parameters)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/SanPhamChiTiet/LayDanhSachTongQuan", parameters);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    return Content(jsonResponse, "application/json");
                }
                else
                {
                    return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LoadPartialViewUpdate(string idSanPhamChiTiet)
        {
            ViewData["IdChatLieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelChatLieuAsync(), "IdChatLieu", "TenChatLieu");
            ViewData["IdKichCo"] = new SelectList((await _SanPhamChiTietservice.GetListModelKichCoAsync()).OrderBy(kc => kc.SoKichCo), "IdKichCo", "SoKichCo");
            ViewData["IdKieuDeGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelKieuDeGiayAsync(), "IdKieuDeGiay", "TenKieuDeGiay");
            ViewData["IdLoaiGiay"] = new SelectList(await _SanPhamChiTietservice.GetListModelLoaiGiayAsync(), "IdLoaiGiay", "TenLoaiGiay");
            ViewData["IdMauSac"] = new SelectList(await _SanPhamChiTietservice.GetListModelMauSacAsync(), "IdMauSac", "TenMauSac");
            ViewData["IdSanPham"] = new SelectList(await _SanPhamChiTietservice.GetListModelSanPhamAsync(), "IdSanPham", "TenSanPham");
            ViewData["IdThuongHieu"] = new SelectList(await _SanPhamChiTietservice.GetListModelThuongHieuAsync(), "IdThuongHieu", "TenThuongHieu");
            ViewData["IdXuatXu"] = new SelectList(await _SanPhamChiTietservice.GetListModelXuatXuAsync(), "IdXuatXu", "Ten");
            var model = await _SanPhamChiTietservice.GetListSanPhamChiTietDTOAsync(new ListGuildDTO()
            {
                listGuild = new List<string>() { idSanPhamChiTiet }
            });
            return PartialView("_SanPhamUpdatePartialView", model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] List<IFormFile> files)
        {
            var formData = new MultipartFormDataContent();
            foreach (var file in files)
            {
                formData.Add(new StreamContent(file.OpenReadStream())
                {
                    Headers =
                {
                    ContentLength = file.Length,
                    ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType)
                }
                }, "files", file.FileName);
            }
            var response = await _httpClient.PostAsync("/api/anh/upload-anh", formData);
            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsAsync<bool>());
            }
            return Ok(false);
        }

    }
}

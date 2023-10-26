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
using App_View.Services;
using App_Data.ViewModels.Voucher;
using App_Data.ViewModels.SanPhamChiTietDTO;
using AutoMapper;
using App_Data.Repositories;
using static App_Data.Repositories.TrangThai;

namespace App_View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VouchersController : Controller
    {
        private readonly BazaizaiContext _context;
        private readonly IVoucherServices _voucherSV;

        public VouchersController(IVoucherServices voucherServices)
        {
            _voucherSV = voucherServices;
            _context = new BazaizaiContext();
        }

        // GET: Admin/Vouchers
        public async Task<IActionResult> Index(string trangThai)
        {
            var lstVoucher = (await _voucherSV.GetAllVoucher()).ToList(); // Lấy tất cả Voucher và chuyển sang List

            // Sắp xếp danh sách ban đầu theo NgayTao, giảm dần
            lstVoucher = lstVoucher.OrderByDescending(c => c.NgayTao).ToList();

            // Tiếp tục với việc lọc dựa trên trạng thái
            if (string.IsNullOrEmpty(trangThai) || trangThai == "hoatDong")
            {
                lstVoucher = lstVoucher.Where(v => v.TrangThai == (int)TrangThaiVoucher.HoatDong).ToList();
            }
            else
            {
                switch (trangThai)
                {
                    case "tatCa":
                        // Không cần thay đổi gì
                        break;
                    case "hoatDong":
                        // Không cần thay đổi gì
                        break;
                    case "khongHoatDong":
                        lstVoucher = lstVoucher.Where(v => v.TrangThai == (int)TrangThaiVoucher.KhongHoatDong).ToList();
                        break;
                    case "chuaBatDau":
                        lstVoucher = lstVoucher.Where(v => v.TrangThai == (int)TrangThaiVoucher.ChuaBatDau).ToList();
                        break;
                    case "DaHuy":
                        lstVoucher = lstVoucher.Where(v => v.TrangThai == (int)TrangThaiVoucher.DaHuy).ToList();
                        break;
                    default:
                        break;
                }
            }

            ViewBag.TatCa = lstVoucher; // Gán danh sách lọc được vào ViewBag.TatCa

            return View(lstVoucher);
        }




        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherDTO voucherDTO)
        {
            if (ModelState.IsValid)
            {
                if (await _voucherSV.CreateVoucher(voucherDTO))
                {
                    return Ok(new { message = " Thêm mới thành công" });
                }
            }
            return Ok(new { error = "Thêm voucher thất bại" });

        }

        public async Task<ActionResult> Edit(string id)
        {
            var Voucher = await _voucherSV.GetVoucherDTOById(id);
            return View(Voucher);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(VoucherDTO voucherDTO)
        {
            if (ModelState.IsValid)
            {
                if (await _voucherSV.UpdateVoucher(voucherDTO))
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        //public async Task<ActionResult> Details(string id)
        //{
        //    var Voucher = (await _voucherSV.GetVoucherDTOById(id));
        //    return View(Voucher);
        //}

        public async Task<ActionResult> Delete(string id)
        {
            await _voucherSV.DeleteVoucher(id);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> DeleteVoucherWithList(List<string> voucherIds)
        {
            if (voucherIds != null)
            {
                await _voucherSV.DeleteVoucherWithList(voucherIds);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> RestoreVoucherWithList(List<string> voucherIds)
        {
            if (voucherIds != null)
            {
                await _voucherSV.RestoreVoucherWithList(voucherIds);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}

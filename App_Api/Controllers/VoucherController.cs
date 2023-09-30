﻿using App_Data.DbContextt;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using App_Data.ViewModels.MauSac;
using App_Data.ViewModels.Voucher;
using AutoMapper;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace App_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IAllRepo<Voucher> allRepo;
        private readonly IMapper _mapper;
        BazaizaiContext DbContextModel = new BazaizaiContext();
        DbSet<Voucher> vouchers;

        public VoucherController(IMapper mapper)
        {
            vouchers = DbContextModel.vouchers;
            AllRepo<Voucher> all = new AllRepo<Voucher>(DbContextModel, vouchers);
            allRepo = all;
            _mapper = mapper;
        }
        [HttpGet("GetVoucher")]
        public List<Voucher> GetAllVoucher()
        {
            return allRepo.GetAll().ToList(); // Wrap the data in an OkObjectResult
        }
        [HttpGet("GetVoucherByMa/{id}")]
        public Voucher? GetVoucher(string id)
        {
            return allRepo.GetAll().FirstOrDefault(c => c.IdVoucher == id);
        }
        private string GenerateRandomVoucherCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            string voucherCode = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return voucherCode;
        }

        [HttpPost("CreateVoucher")]
        public bool Create(VoucherDTO voucherDTO)
        {
            voucherDTO.IdVoucher = Guid.NewGuid().ToString();
            var voucher = _mapper.Map<Voucher>(voucherDTO);
            voucher.MaVoucher = GenerateRandomVoucherCode();
            if (voucher.NgayBatDau > DateTime.Now)
            {
                voucher.TrangThai = 1;
            }
            else
                voucher.TrangThai = 0;
            return allRepo.AddItem(voucher);
        }
        [HttpDelete("DeleteVoucher/{id}")]
        public bool Delete(string id)
        {
            var VoucherGet = GetVoucher(id);
            if (VoucherGet != null)
            {
                VoucherGet!.TrangThai = 1;
                return allRepo.EditItem(VoucherGet);
            }
            return false;
        }
        [HttpPut("UpdateVoucher")]
        public bool Update(VoucherDTO voucherDTO)
        {
            var voucherGet = GetVoucher(voucherDTO.IdVoucher);
            if (voucherGet != null)
            {
                _mapper.Map(voucherDTO, voucherGet);
                if (voucherGet.NgayBatDau > DateTime.Now)
                {
                    voucherGet.TrangThai = 1;
                }
                else
                    voucherGet.TrangThai = 0;
                return allRepo.EditItem(voucherGet);
            }
            return false;
        }
    }
}
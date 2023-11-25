using App_Data.Models;
using App_View.Services;
using App_View.IServices;
using EnumsNET;
using Microsoft.AspNetCore.Identity;
using System;
using static App_Data.Repositories.TrangThai;

namespace App_View.Models
{
    public class ContextdDefault
    {
        public static async Task SeedRolesAsync(UserManager<NguoiDung> userManager, RoleManager<ChucVu> roleManager)
        {
            var rolesToSeed = new[]{
                new ChucVu { Name = ChucVuMacDinh.KhachHang.ToString(), Id = Guid.NewGuid().ToString(),TrangThai=(int?)TrangThaiCoBan.HoatDong,MaChucVu="CV1" },
                new ChucVu { Name = ChucVuMacDinh.Admin.ToString(), Id = Guid.NewGuid().ToString(),TrangThai=(int?)TrangThaiCoBan.HoatDong,MaChucVu="CV2" },
                new ChucVu { Name = ChucVuMacDinh.NhanVien.ToString(), Id = Guid.NewGuid().ToString(),TrangThai=(int?)TrangThaiCoBan.HoatDong,MaChucVu="CV3" }
            };

            foreach (var role in rolesToSeed)
            {
                await roleManager.CreateAsync(role);
            }
        }
        public static async Task SeeAdminAsync(UserManager<NguoiDung> userManager, RoleManager<ChucVu> roleManager)
        {
            ////Seed Default User
            var defaultUser = new NguoiDung
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                Email = "bazaizaistore@gmail.com",
                TenNguoiDung = "Admin",
                PhoneNumber = "0369426223",
                EmailConfirmed = true,
                NgaySinh = DateTime.ParseExact("10-10-2010", "MM-dd-yyyy", null),
                DiaChi = "Hà Nội",
                MaNguoiDung = "ND1",
                AnhDaiDien = "/user_img/default_image.png",
                TrangThai = (int?)TrangThaiCoBan.HoatDong,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123456");
                    await userManager.AddToRoleAsync(defaultUser, ChucVuMacDinh.Admin.ToString());
                }

            }
        }
        public static async Task PhuongThucThanhToan()
        {
            IPTThanhToanServices _pTThanhToanServices = new PTThanhToanServices();
            await _pTThanhToanServices.CreatePTThanhToanAsync("COD", "COD", 0);
            await _pTThanhToanServices.CreatePTThanhToanAsync("MOMO", "MOMO", 0);
            await _pTThanhToanServices.CreatePTThanhToanAsync("VNPAY", "VNPAY", 0);
            await _pTThanhToanServices.CreatePTThanhToanAsync("TienMat", "Tai quay", 0);
            await _pTThanhToanServices.CreatePTThanhToanAsync("ChuyenKhoan", "Tai quay", 0);
        }
    }
}

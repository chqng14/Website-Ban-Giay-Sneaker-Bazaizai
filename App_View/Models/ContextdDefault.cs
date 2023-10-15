using App_Data.Models;
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
                Id= Guid.NewGuid().ToString(),  
                UserName = "Admin",
                Email = "adminhehehe@gmail.com",
                TenNguoiDung = "Mi Mi",
                PhoneNumber = "0369426223",
                EmailConfirmed = true,
                NgaySinh= DateTime.ParseExact("10-10-2010", "MM-dd-yyyy", null),
                //AnhDaiDien=
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "uchihahaha123");
                    await userManager.AddToRoleAsync(defaultUser, ChucVuMacDinh.Admin.ToString());
                }

            }
        }
    }
}

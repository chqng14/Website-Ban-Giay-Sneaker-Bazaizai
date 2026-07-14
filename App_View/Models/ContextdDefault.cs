using App_Data.Models;
using App_View.Services;
using App_View.IServices;
using EnumsNET;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
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
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
                    }
                }
            }
        }
        public static async Task SeedAdminAsync(UserManager<NguoiDung> userManager, IConfiguration configuration)
        {
            var email = configuration["SeedAdmin:Email"];
            var password = configuration["SeedAdmin:Password"];
            var userName = configuration["SeedAdmin:UserName"] ?? "Admin";
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return;
            }

            var defaultUser = new NguoiDung
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName,
                Email = email,
                TenNguoiDung = "Admin",
                EmailConfirmed = true,
                NgaySinh = DateTime.ParseExact("10-10-2010", "MM-dd-yyyy", null),
                DiaChi = "Hà Nội",
                MaNguoiDung = "ND1",
                AnhDaiDien = "/user_img/default_image.png",
                TrangThai = (int?)TrangThaiCoBan.HoatDong,
                PhoneNumberConfirmed = true
            };
            var user = await userManager.FindByEmailAsync(email);
            var created = false;
            if (user == null)
            {
                var result = await userManager.CreateAsync(defaultUser, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join("; ", result.Errors.Select(x => x.Description)));
                }
                user = defaultUser;
                created = true;
            }

            if (!string.Equals(user.UserName, userName, StringComparison.Ordinal))
            {
                var userNameResult = await userManager.SetUserNameAsync(user, userName);
                if (!userNameResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ", userNameResult.Errors.Select(x => x.Description)));
                }
            }

            if (!created && configuration.GetValue<bool>("SeedAdmin:EnsurePassword"))
            {
                var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await userManager.ResetPasswordAsync(user, resetToken, password);
                if (!resetResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ", resetResult.Errors.Select(x => x.Description)));
                }
            }

            var changed = false;
            if (!user.EmailConfirmed || !user.PhoneNumberConfirmed
                || user.TwoFactorEnabled
                || user.LockoutEnd != null
                || user.AccessFailedCount != 0
                || user.TrangThai != (int?)TrangThaiCoBan.HoatDong)
            {
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;
                user.TwoFactorEnabled = false;
                user.LockoutEnd = null;
                user.AccessFailedCount = 0;
                user.TrangThai = (int?)TrangThaiCoBan.HoatDong;
                changed = true;
            }

            if (changed)
            {
                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ", updateResult.Errors.Select(x => x.Description)));
                }
            }

            if ((created || configuration.GetValue<bool>("SeedAdmin:EnsurePassword"))
                && !await userManager.CheckPasswordAsync(user, password))
            {
                throw new InvalidOperationException("Không thể xác nhận mật khẩu của tài khoản admin seed.");
            }

            if (!await userManager.IsInRoleAsync(user, ChucVuMacDinh.Admin.ToString()))
            {
                var roleResult = await userManager.AddToRoleAsync(
                    user,
                    ChucVuMacDinh.Admin.ToString());
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException(
                        string.Join("; ", roleResult.Errors.Select(x => x.Description)));
                }
            }
        }
        public static async Task PhuongThucThanhToan(IPTThanhToanServices paymentService)
        {
            await paymentService.CreatePTThanhToanAsync("COD", "COD", 0);
            await paymentService.CreatePTThanhToanAsync("MOMO", "MOMO", 0);
            await paymentService.CreatePTThanhToanAsync("VNPAY", "VNPAY", 0);
            await paymentService.CreatePTThanhToanAsync("TienMat", "Tai quay", 0);
            await paymentService.CreatePTThanhToanAsync("ChuyenKhoan", "Tai quay", 0);
        }
    }
}

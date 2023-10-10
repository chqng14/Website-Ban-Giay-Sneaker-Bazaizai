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
                new ChucVu { Name = ChucVuMacDinh.KhachHang.ToString(), Id = Guid.NewGuid().ToString() },
                new ChucVu { Name = ChucVuMacDinh.Admin.ToString(), Id = Guid.NewGuid().ToString() },
                new ChucVu { Name = ChucVuMacDinh.NhanVien.ToString(), Id = Guid.NewGuid().ToString() }

            };

            foreach (var role in rolesToSeed)
            {
                await roleManager.CreateAsync(role);
            }
        }
        public static async Task SeedSuperAdminAsync(UserManager<NguoiDung> userManager, RoleManager<ChucVu> roleManager)
        {
            ////Seed Default User
            //var defaultUser = new NguoiDung
            //{
            //    UserName = "superadmin",
            //    Email = "superadmin@gmail.com",
            //    FirstName = "Mukesh",
            //    LastName = "Murugan",
            //    EmailConfirmed = true,
            //    PhoneNumberConfirmed = true
            //};
            //if (userManager.Users.All(u => u.Id != defaultUser.Id))
            //{
            //    var user = await userManager.FindByEmailAsync(defaultUser.Email);
            //    if (user == null)
            //    {
            //        await userManager.CreateAsync(defaultUser, "123Pa$$word.");
            //        await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Basic.ToString());
            //        await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Moderator.ToString());
            //        await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
            //        await userManager.AddToRoleAsync(defaultUser, Enums.Roles.SuperAdmin.ToString());
            //    }

            //}
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class _17_10_2023_SuaTenBang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "Anh");

            migrationBuilder.DropForeignKey(
                name: "FK_Đánh giá_Đánh giá_ParentId",
                table: "Đánh giá");

            migrationBuilder.DropForeignKey(
                name: "FK_Đánh giá_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "Đánh giá");

            migrationBuilder.DropForeignKey(
                name: "FK_Đánh giá_Users_IdNguoiDung",
                table: "Đánh giá");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHangChiTiets_GioHangs_IdNguoiDung",
                table: "GioHangChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHangChiTiets_SanPhamChiTiets_IdSanPhamCT",
                table: "GioHangChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHangs_Users_NguoiDungId",
                table: "GioHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonChiTiets_HoaDons_IdHoaDon",
                table: "HoaDonChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonChiTiets_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "HoaDonChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhachHang_IdKhachHang",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_ThongTinGiaoHangs_IdThongTinGH",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_Users_IdNguoiDung",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_Vouchers_IdVoucher",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_Users_IdNguoiDung",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_PhuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                table: "PhuongThucThanhToanChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_PhuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                table: "PhuongThucThanhToanChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiets_ChatLieus_IdChatLieu",
                table: "SanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiets_KichCos_IdKichCo",
                table: "SanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiets_KieuDeGiays_IdKieuDeGiay",
                table: "SanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiets_LoaiGiays_IdLoaiGiay",
                table: "SanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiets_MauSacs_IdMauSac",
                table: "SanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiets_SanPhams_IdSanPham",
                table: "SanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiets_ThuongHieus_IdThuongHieu",
                table: "SanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiets_XuatXus_IdXuatXu",
                table: "SanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamYeuThichs_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "SanPhamYeuThichs");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamYeuThichs_Users_IdNguoiDung",
                table: "SanPhamYeuThichs");

            migrationBuilder.DropForeignKey(
                name: "FK_ThongTinGiaoHangs_Users_IdNguoiDung",
                table: "ThongTinGiaoHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserClaims_Users_UserId",
                table: "UserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLogins_Users_UserId",
                table: "UserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherNguoiDungs_Users_IdNguoiDung",
                table: "VoucherNguoiDungs");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherNguoiDungs_Vouchers_IdVouCher",
                table: "VoucherNguoiDungs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_XuatXus",
                table: "XuatXus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vouchers",
                table: "Vouchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoucherNguoiDungs",
                table: "VoucherNguoiDungs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThuongHieus",
                table: "ThuongHieus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongTinGiaoHangs",
                table: "ThongTinGiaoHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPhamYeuThichs",
                table: "SanPhamYeuThichs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPhams",
                table: "SanPhams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPhamChiTiets",
                table: "SanPhamChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Roles",
                table: "Roles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleClaims",
                table: "RoleClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhuongThucThanhToans",
                table: "PhuongThucThanhToans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhuongThucThanhToanChiTiets",
                table: "PhuongThucThanhToanChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MauSacs",
                table: "MauSacs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiGiays",
                table: "LoaiGiays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KieuDeGiays",
                table: "KieuDeGiays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KichCos",
                table: "KichCos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDons",
                table: "HoaDons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDonChiTiets",
                table: "HoaDonChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GioHangs",
                table: "GioHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GioHangChiTiets",
                table: "GioHangChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Đánh giá",
                table: "Đánh giá");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatLieus",
                table: "ChatLieus");

            migrationBuilder.RenameTable(
                name: "XuatXus",
                newName: "XuatXu");

            migrationBuilder.RenameTable(
                name: "Vouchers",
                newName: "Voucher");

            migrationBuilder.RenameTable(
                name: "VoucherNguoiDungs",
                newName: "VoucherNguoiDung");

            migrationBuilder.RenameTable(
                name: "UserTokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "NguoiDung");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "UserLogins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "UserClaims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "ThuongHieus",
                newName: "ThuongHieu");

            migrationBuilder.RenameTable(
                name: "ThongTinGiaoHangs",
                newName: "ThongTinGiaoHang");

            migrationBuilder.RenameTable(
                name: "SanPhamYeuThichs",
                newName: "SanPhamYeuThich");

            migrationBuilder.RenameTable(
                name: "SanPhams",
                newName: "SanPham");

            migrationBuilder.RenameTable(
                name: "SanPhamChiTiets",
                newName: "SanPhamChiTiet");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "ChucVu");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "PhuongThucThanhToans",
                newName: "PhuongThucThanhToan");

            migrationBuilder.RenameTable(
                name: "PhuongThucThanhToanChiTiets",
                newName: "PhuongThucThanhToanChiTiet");

            migrationBuilder.RenameTable(
                name: "MauSacs",
                newName: "MauSac");

            migrationBuilder.RenameTable(
                name: "LoaiGiays",
                newName: "LoaiGiay");

            migrationBuilder.RenameTable(
                name: "KieuDeGiays",
                newName: "KieuDeGiay");

            migrationBuilder.RenameTable(
                name: "KichCos",
                newName: "KichCo");

            migrationBuilder.RenameTable(
                name: "HoaDons",
                newName: "HoaDon");

            migrationBuilder.RenameTable(
                name: "HoaDonChiTiets",
                newName: "HoaDonChiTiet");

            migrationBuilder.RenameTable(
                name: "GioHangs",
                newName: "GioHang");

            migrationBuilder.RenameTable(
                name: "GioHangChiTiets",
                newName: "GioHangChiTiet");

            migrationBuilder.RenameTable(
                name: "Đánh giá",
                newName: "DanhGia");

            migrationBuilder.RenameTable(
                name: "ChatLieus",
                newName: "ChatLieu");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherNguoiDungs_IdVouCher",
                table: "VoucherNguoiDung",
                newName: "IX_VoucherNguoiDung_IdVouCher");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherNguoiDungs_IdNguoiDung",
                table: "VoucherNguoiDung",
                newName: "IX_VoucherNguoiDung_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoles_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_UserLogins_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserClaims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ThongTinGiaoHangs_IdNguoiDung",
                table: "ThongTinGiaoHang",
                newName: "IX_ThongTinGiaoHang_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamYeuThichs_IdSanPhamChiTiet",
                table: "SanPhamYeuThich",
                newName: "IX_SanPhamYeuThich_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamYeuThichs_IdNguoiDung",
                table: "SanPhamYeuThich",
                newName: "IX_SanPhamYeuThich_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiets_IdXuatXu",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdXuatXu");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiets_IdThuongHieu",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdThuongHieu");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiets_IdSanPham",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdSanPham");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiets_IdMauSac",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdMauSac");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiets_IdLoaiGiay",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdLoaiGiay");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiets_IdKieuDeGiay",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdKieuDeGiay");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiets_IdKichCo",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdKichCo");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiets_IdChatLieu",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdChatLieu");

            migrationBuilder.RenameIndex(
                name: "IX_RoleClaims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_PhuongThucThanhToanChiTiets_IdThanhToan",
                table: "PhuongThucThanhToanChiTiet",
                newName: "IX_PhuongThucThanhToanChiTiet_IdThanhToan");

            migrationBuilder.RenameIndex(
                name: "IX_PhuongThucThanhToanChiTiets_IdHoaDon",
                table: "PhuongThucThanhToanChiTiet",
                newName: "IX_PhuongThucThanhToanChiTiet_IdHoaDon");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_IdVoucher",
                table: "HoaDon",
                newName: "IX_HoaDon_IdVoucher");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_IdThongTinGH",
                table: "HoaDon",
                newName: "IX_HoaDon_IdThongTinGH");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_IdNguoiDung",
                table: "HoaDon",
                newName: "IX_HoaDon_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDons_IdKhachHang",
                table: "HoaDon",
                newName: "IX_HoaDon_IdKhachHang");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDonChiTiets_IdSanPhamChiTiet",
                table: "HoaDonChiTiet",
                newName: "IX_HoaDonChiTiet_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDonChiTiets_IdHoaDon",
                table: "HoaDonChiTiet",
                newName: "IX_HoaDonChiTiet_IdHoaDon");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangs_NguoiDungId",
                table: "GioHang",
                newName: "IX_GioHang_NguoiDungId");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiets_IdSanPhamCT",
                table: "GioHangChiTiet",
                newName: "IX_GioHangChiTiet_IdSanPhamCT");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiets_IdNguoiDung",
                table: "GioHangChiTiet",
                newName: "IX_GioHangChiTiet_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_Đánh giá_ParentId",
                table: "DanhGia",
                newName: "IX_DanhGia_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Đánh giá_IdSanPhamChiTiet",
                table: "DanhGia",
                newName: "IX_DanhGia_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_Đánh giá_IdNguoiDung",
                table: "DanhGia",
                newName: "IX_DanhGia_IdNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_XuatXu",
                table: "XuatXu",
                column: "IdXuatXu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voucher",
                table: "Voucher",
                column: "IdVoucher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoucherNguoiDung",
                table: "VoucherNguoiDung",
                column: "IdVouCherNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_NguoiDung",
                table: "NguoiDung",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThuongHieu",
                table: "ThuongHieu",
                column: "IdThuongHieu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongTinGiaoHang",
                table: "ThongTinGiaoHang",
                column: "IdThongTinGH");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPhamYeuThich",
                table: "SanPhamYeuThich",
                column: "IdSanPhamYeuThich");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPham",
                table: "SanPham",
                column: "IdSanPham");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPhamChiTiet",
                table: "SanPhamChiTiet",
                column: "IdChiTietSp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChucVu",
                table: "ChucVu",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhuongThucThanhToan",
                table: "PhuongThucThanhToan",
                column: "IdPhuongThucThanhToan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhuongThucThanhToanChiTiet",
                table: "PhuongThucThanhToanChiTiet",
                column: "IdPhuongThucThanhToanChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MauSac",
                table: "MauSac",
                column: "IdMauSac");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiGiay",
                table: "LoaiGiay",
                column: "IdLoaiGiay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KieuDeGiay",
                table: "KieuDeGiay",
                column: "IdKieuDeGiay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KichCo",
                table: "KichCo",
                column: "IdKichCo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDon",
                table: "HoaDon",
                column: "IdHoaDon");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDonChiTiet",
                table: "HoaDonChiTiet",
                column: "IdHoaDonChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GioHang",
                table: "GioHang",
                column: "IdNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GioHangChiTiet",
                table: "GioHangChiTiet",
                column: "IdGioHangChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DanhGia",
                table: "DanhGia",
                column: "IdDanhGia");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatLieu",
                table: "ChatLieu",
                column: "IdChatLieu");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "Anh",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiet",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_ChucVu_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "ChucVu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_NguoiDung_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_NguoiDung_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_ChucVu_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "ChucVu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_NguoiDung_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_NguoiDung_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "NguoiDung",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DanhGia_DanhGia_ParentId",
            //    table: "DanhGia",
            //    column: "ParentId",
            //    principalTable: "DanhGia",
            //    principalColumn: "IdDanhGia");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGia_NguoiDung_IdNguoiDung",
                table: "DanhGia",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGia_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "DanhGia",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiet",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHang_NguoiDung_NguoiDungId",
                table: "GioHang",
                column: "NguoiDungId",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangChiTiet_GioHang_IdNguoiDung",
                table: "GioHangChiTiet",
                column: "IdNguoiDung",
                principalTable: "GioHang",
                principalColumn: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangChiTiet_SanPhamChiTiet_IdSanPhamCT",
                table: "GioHangChiTiet",
                column: "IdSanPhamCT",
                principalTable: "SanPhamChiTiet",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_KhachHang_IdKhachHang",
                table: "HoaDon",
                column: "IdKhachHang",
                principalTable: "KhachHang",
                principalColumn: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_NguoiDung_IdNguoiDung",
                table: "HoaDon",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_ThongTinGiaoHang_IdThongTinGH",
                table: "HoaDon",
                column: "IdThongTinGH",
                principalTable: "ThongTinGiaoHang",
                principalColumn: "IdThongTinGH");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDon_Voucher_IdVoucher",
                table: "HoaDon",
                column: "IdVoucher",
                principalTable: "Voucher",
                principalColumn: "IdVoucher");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonChiTiet_HoaDon_IdHoaDon",
                table: "HoaDonChiTiet",
                column: "IdHoaDon",
                principalTable: "HoaDon",
                principalColumn: "IdHoaDon");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonChiTiet_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "HoaDonChiTiet",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiet",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_NguoiDung_IdNguoiDung",
                table: "KhachHang",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiet",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_PhuongThucThanhToanChiTiet_HoaDon_IdHoaDon",
                table: "PhuongThucThanhToanChiTiet",
                column: "IdHoaDon",
                principalTable: "HoaDon",
                principalColumn: "IdHoaDon");

            migrationBuilder.AddForeignKey(
                name: "FK_PhuongThucThanhToanChiTiet_PhuongThucThanhToan_IdThanhToan",
                table: "PhuongThucThanhToanChiTiet",
                column: "IdThanhToan",
                principalTable: "PhuongThucThanhToan",
                principalColumn: "IdPhuongThucThanhToan");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_ChatLieu_IdChatLieu",
                table: "SanPhamChiTiet",
                column: "IdChatLieu",
                principalTable: "ChatLieu",
                principalColumn: "IdChatLieu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_KichCo_IdKichCo",
                table: "SanPhamChiTiet",
                column: "IdKichCo",
                principalTable: "KichCo",
                principalColumn: "IdKichCo");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_KieuDeGiay_IdKieuDeGiay",
                table: "SanPhamChiTiet",
                column: "IdKieuDeGiay",
                principalTable: "KieuDeGiay",
                principalColumn: "IdKieuDeGiay");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_LoaiGiay_IdLoaiGiay",
                table: "SanPhamChiTiet",
                column: "IdLoaiGiay",
                principalTable: "LoaiGiay",
                principalColumn: "IdLoaiGiay");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_MauSac_IdMauSac",
                table: "SanPhamChiTiet",
                column: "IdMauSac",
                principalTable: "MauSac",
                principalColumn: "IdMauSac");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_SanPham_IdSanPham",
                table: "SanPhamChiTiet",
                column: "IdSanPham",
                principalTable: "SanPham",
                principalColumn: "IdSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_ThuongHieu_IdThuongHieu",
                table: "SanPhamChiTiet",
                column: "IdThuongHieu",
                principalTable: "ThuongHieu",
                principalColumn: "IdThuongHieu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_XuatXu_IdXuatXu",
                table: "SanPhamChiTiet",
                column: "IdXuatXu",
                principalTable: "XuatXu",
                principalColumn: "IdXuatXu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamYeuThich_NguoiDung_IdNguoiDung",
                table: "SanPhamYeuThich",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamYeuThich_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "SanPhamYeuThich",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiet",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongTinGiaoHang_NguoiDung_IdNguoiDung",
                table: "ThongTinGiaoHang",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherNguoiDung_NguoiDung_IdNguoiDung",
                table: "VoucherNguoiDung",
                column: "IdNguoiDung",
                principalTable: "NguoiDung",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherNguoiDung_Voucher_IdVouCher",
                table: "VoucherNguoiDung",
                column: "IdVouCher",
                principalTable: "Voucher",
                principalColumn: "IdVoucher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "Anh");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_ChucVu_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_NguoiDung_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_NguoiDung_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_ChucVu_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_NguoiDung_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_NguoiDung_UserId",
                table: "AspNetUserTokens");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_DanhGia_DanhGia_ParentId",
            //    table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGia_NguoiDung_IdNguoiDung",
                table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGia_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHang_NguoiDung_NguoiDungId",
                table: "GioHang");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHangChiTiet_GioHang_IdNguoiDung",
                table: "GioHangChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_GioHangChiTiet_SanPhamChiTiet_IdSanPhamCT",
                table: "GioHangChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_KhachHang_IdKhachHang",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_NguoiDung_IdNguoiDung",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_ThongTinGiaoHang_IdThongTinGH",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDon_Voucher_IdVoucher",
                table: "HoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonChiTiet_HoaDon_IdHoaDon",
                table: "HoaDonChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonChiTiet_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "HoaDonChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_NguoiDung_IdNguoiDung",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_PhuongThucThanhToanChiTiet_HoaDon_IdHoaDon",
                table: "PhuongThucThanhToanChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_PhuongThucThanhToanChiTiet_PhuongThucThanhToan_IdThanhToan",
                table: "PhuongThucThanhToanChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiet_ChatLieu_IdChatLieu",
                table: "SanPhamChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiet_KichCo_IdKichCo",
                table: "SanPhamChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiet_KieuDeGiay_IdKieuDeGiay",
                table: "SanPhamChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiet_LoaiGiay_IdLoaiGiay",
                table: "SanPhamChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiet_MauSac_IdMauSac",
                table: "SanPhamChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiet_SanPham_IdSanPham",
                table: "SanPhamChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiet_ThuongHieu_IdThuongHieu",
                table: "SanPhamChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamChiTiet_XuatXu_IdXuatXu",
                table: "SanPhamChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamYeuThich_NguoiDung_IdNguoiDung",
                table: "SanPhamYeuThich");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPhamYeuThich_SanPhamChiTiet_IdSanPhamChiTiet",
                table: "SanPhamYeuThich");

            migrationBuilder.DropForeignKey(
                name: "FK_ThongTinGiaoHang_NguoiDung_IdNguoiDung",
                table: "ThongTinGiaoHang");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherNguoiDung_NguoiDung_IdNguoiDung",
                table: "VoucherNguoiDung");

            migrationBuilder.DropForeignKey(
                name: "FK_VoucherNguoiDung_Voucher_IdVouCher",
                table: "VoucherNguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_XuatXu",
                table: "XuatXu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VoucherNguoiDung",
                table: "VoucherNguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voucher",
                table: "Voucher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThuongHieu",
                table: "ThuongHieu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ThongTinGiaoHang",
                table: "ThongTinGiaoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPhamYeuThich",
                table: "SanPhamYeuThich");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPhamChiTiet",
                table: "SanPhamChiTiet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPham",
                table: "SanPham");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhuongThucThanhToanChiTiet",
                table: "PhuongThucThanhToanChiTiet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PhuongThucThanhToan",
                table: "PhuongThucThanhToan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NguoiDung",
                table: "NguoiDung");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MauSac",
                table: "MauSac");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiGiay",
                table: "LoaiGiay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KieuDeGiay",
                table: "KieuDeGiay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KichCo",
                table: "KichCo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDonChiTiet",
                table: "HoaDonChiTiet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDon",
                table: "HoaDon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GioHangChiTiet",
                table: "GioHangChiTiet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GioHang",
                table: "GioHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DanhGia",
                table: "DanhGia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChucVu",
                table: "ChucVu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatLieu",
                table: "ChatLieu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "XuatXu",
                newName: "XuatXus");

            migrationBuilder.RenameTable(
                name: "VoucherNguoiDung",
                newName: "VoucherNguoiDungs");

            migrationBuilder.RenameTable(
                name: "Voucher",
                newName: "Vouchers");

            migrationBuilder.RenameTable(
                name: "ThuongHieu",
                newName: "ThuongHieus");

            migrationBuilder.RenameTable(
                name: "ThongTinGiaoHang",
                newName: "ThongTinGiaoHangs");

            migrationBuilder.RenameTable(
                name: "SanPhamYeuThich",
                newName: "SanPhamYeuThichs");

            migrationBuilder.RenameTable(
                name: "SanPhamChiTiet",
                newName: "SanPhamChiTiets");

            migrationBuilder.RenameTable(
                name: "SanPham",
                newName: "SanPhams");

            migrationBuilder.RenameTable(
                name: "PhuongThucThanhToanChiTiet",
                newName: "PhuongThucThanhToanChiTiets");

            migrationBuilder.RenameTable(
                name: "PhuongThucThanhToan",
                newName: "PhuongThucThanhToans");

            migrationBuilder.RenameTable(
                name: "NguoiDung",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "MauSac",
                newName: "MauSacs");

            migrationBuilder.RenameTable(
                name: "LoaiGiay",
                newName: "LoaiGiays");

            migrationBuilder.RenameTable(
                name: "KieuDeGiay",
                newName: "KieuDeGiays");

            migrationBuilder.RenameTable(
                name: "KichCo",
                newName: "KichCos");

            migrationBuilder.RenameTable(
                name: "HoaDonChiTiet",
                newName: "HoaDonChiTiets");

            migrationBuilder.RenameTable(
                name: "HoaDon",
                newName: "HoaDons");

            migrationBuilder.RenameTable(
                name: "GioHangChiTiet",
                newName: "GioHangChiTiets");

            migrationBuilder.RenameTable(
                name: "GioHang",
                newName: "GioHangs");

            migrationBuilder.RenameTable(
                name: "DanhGia",
                newName: "Đánh giá");

            migrationBuilder.RenameTable(
                name: "ChucVu",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "ChatLieu",
                newName: "ChatLieus");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "UserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "UserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "UserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "RoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherNguoiDung_IdVouCher",
                table: "VoucherNguoiDungs",
                newName: "IX_VoucherNguoiDungs_IdVouCher");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherNguoiDung_IdNguoiDung",
                table: "VoucherNguoiDungs",
                newName: "IX_VoucherNguoiDungs_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_ThongTinGiaoHang_IdNguoiDung",
                table: "ThongTinGiaoHangs",
                newName: "IX_ThongTinGiaoHangs_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamYeuThich_IdSanPhamChiTiet",
                table: "SanPhamYeuThichs",
                newName: "IX_SanPhamYeuThichs_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamYeuThich_IdNguoiDung",
                table: "SanPhamYeuThichs",
                newName: "IX_SanPhamYeuThichs_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdXuatXu",
                table: "SanPhamChiTiets",
                newName: "IX_SanPhamChiTiets_IdXuatXu");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdThuongHieu",
                table: "SanPhamChiTiets",
                newName: "IX_SanPhamChiTiets_IdThuongHieu");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdSanPham",
                table: "SanPhamChiTiets",
                newName: "IX_SanPhamChiTiets_IdSanPham");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdMauSac",
                table: "SanPhamChiTiets",
                newName: "IX_SanPhamChiTiets_IdMauSac");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdLoaiGiay",
                table: "SanPhamChiTiets",
                newName: "IX_SanPhamChiTiets_IdLoaiGiay");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdKieuDeGiay",
                table: "SanPhamChiTiets",
                newName: "IX_SanPhamChiTiets_IdKieuDeGiay");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdKichCo",
                table: "SanPhamChiTiets",
                newName: "IX_SanPhamChiTiets_IdKichCo");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdChatLieu",
                table: "SanPhamChiTiets",
                newName: "IX_SanPhamChiTiets_IdChatLieu");

            migrationBuilder.RenameIndex(
                name: "IX_PhuongThucThanhToanChiTiet_IdThanhToan",
                table: "PhuongThucThanhToanChiTiets",
                newName: "IX_PhuongThucThanhToanChiTiets_IdThanhToan");

            migrationBuilder.RenameIndex(
                name: "IX_PhuongThucThanhToanChiTiet_IdHoaDon",
                table: "PhuongThucThanhToanChiTiets",
                newName: "IX_PhuongThucThanhToanChiTiets_IdHoaDon");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDonChiTiet_IdSanPhamChiTiet",
                table: "HoaDonChiTiets",
                newName: "IX_HoaDonChiTiets_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDonChiTiet_IdHoaDon",
                table: "HoaDonChiTiets",
                newName: "IX_HoaDonChiTiets_IdHoaDon");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_IdVoucher",
                table: "HoaDons",
                newName: "IX_HoaDons_IdVoucher");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_IdThongTinGH",
                table: "HoaDons",
                newName: "IX_HoaDons_IdThongTinGH");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_IdNguoiDung",
                table: "HoaDons",
                newName: "IX_HoaDons_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDon_IdKhachHang",
                table: "HoaDons",
                newName: "IX_HoaDons_IdKhachHang");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiet_IdSanPhamCT",
                table: "GioHangChiTiets",
                newName: "IX_GioHangChiTiets_IdSanPhamCT");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiet_IdNguoiDung",
                table: "GioHangChiTiets",
                newName: "IX_GioHangChiTiets_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_GioHang_NguoiDungId",
                table: "GioHangs",
                newName: "IX_GioHangs_NguoiDungId");

            migrationBuilder.RenameIndex(
                name: "IX_DanhGia_ParentId",
                table: "Đánh giá",
                newName: "IX_Đánh giá_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_DanhGia_IdSanPhamChiTiet",
                table: "Đánh giá",
                newName: "IX_Đánh giá_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_DanhGia_IdNguoiDung",
                table: "Đánh giá",
                newName: "IX_Đánh giá_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "UserRoles",
                newName: "IX_UserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "UserLogins",
                newName: "IX_UserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "UserClaims",
                newName: "IX_UserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "RoleClaims",
                newName: "IX_RoleClaims_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_XuatXus",
                table: "XuatXus",
                column: "IdXuatXu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VoucherNguoiDungs",
                table: "VoucherNguoiDungs",
                column: "IdVouCherNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vouchers",
                table: "Vouchers",
                column: "IdVoucher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThuongHieus",
                table: "ThuongHieus",
                column: "IdThuongHieu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ThongTinGiaoHangs",
                table: "ThongTinGiaoHangs",
                column: "IdThongTinGH");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPhamYeuThichs",
                table: "SanPhamYeuThichs",
                column: "IdSanPhamYeuThich");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPhamChiTiets",
                table: "SanPhamChiTiets",
                column: "IdChiTietSp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPhams",
                table: "SanPhams",
                column: "IdSanPham");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhuongThucThanhToanChiTiets",
                table: "PhuongThucThanhToanChiTiets",
                column: "IdPhuongThucThanhToanChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PhuongThucThanhToans",
                table: "PhuongThucThanhToans",
                column: "IdPhuongThucThanhToan");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MauSacs",
                table: "MauSacs",
                column: "IdMauSac");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiGiays",
                table: "LoaiGiays",
                column: "IdLoaiGiay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KieuDeGiays",
                table: "KieuDeGiays",
                column: "IdKieuDeGiay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KichCos",
                table: "KichCos",
                column: "IdKichCo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDonChiTiets",
                table: "HoaDonChiTiets",
                column: "IdHoaDonChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDons",
                table: "HoaDons",
                column: "IdHoaDon");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GioHangChiTiets",
                table: "GioHangChiTiets",
                column: "IdGioHangChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GioHangs",
                table: "GioHangs",
                column: "IdNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Đánh giá",
                table: "Đánh giá",
                column: "IdDanhGia");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Roles",
                table: "Roles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatLieus",
                table: "ChatLieus",
                column: "IdChatLieu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTokens",
                table: "UserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoles",
                table: "UserRoles",
                columns: new[] { "UserId", "RoleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserLogins",
                table: "UserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserClaims",
                table: "UserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleClaims",
                table: "RoleClaims",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "Anh",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_Đánh giá_Đánh giá_ParentId",
                table: "Đánh giá",
                column: "ParentId",
                principalTable: "Đánh giá",
                principalColumn: "IdDanhGia");

            migrationBuilder.AddForeignKey(
                name: "FK_Đánh giá_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "Đánh giá",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_Đánh giá_Users_IdNguoiDung",
                table: "Đánh giá",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangChiTiets_GioHangs_IdNguoiDung",
                table: "GioHangChiTiets",
                column: "IdNguoiDung",
                principalTable: "GioHangs",
                principalColumn: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangChiTiets_SanPhamChiTiets_IdSanPhamCT",
                table: "GioHangChiTiets",
                column: "IdSanPhamCT",
                principalTable: "SanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_GioHangs_Users_NguoiDungId",
                table: "GioHangs",
                column: "NguoiDungId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonChiTiets_HoaDons_IdHoaDon",
                table: "HoaDonChiTiets",
                column: "IdHoaDon",
                principalTable: "HoaDons",
                principalColumn: "IdHoaDon");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonChiTiets_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "HoaDonChiTiets",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhachHang_IdKhachHang",
                table: "HoaDons",
                column: "IdKhachHang",
                principalTable: "KhachHang",
                principalColumn: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_ThongTinGiaoHangs_IdThongTinGH",
                table: "HoaDons",
                column: "IdThongTinGH",
                principalTable: "ThongTinGiaoHangs",
                principalColumn: "IdThongTinGH");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_Users_IdNguoiDung",
                table: "HoaDons",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_Vouchers_IdVoucher",
                table: "HoaDons",
                column: "IdVoucher",
                principalTable: "Vouchers",
                principalColumn: "IdVoucher");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_Users_IdNguoiDung",
                table: "KhachHang",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_PhuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                table: "PhuongThucThanhToanChiTiets",
                column: "IdHoaDon",
                principalTable: "HoaDons",
                principalColumn: "IdHoaDon");

            migrationBuilder.AddForeignKey(
                name: "FK_PhuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                table: "PhuongThucThanhToanChiTiets",
                column: "IdThanhToan",
                principalTable: "PhuongThucThanhToans",
                principalColumn: "IdPhuongThucThanhToan");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                table: "RoleClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiets_ChatLieus_IdChatLieu",
                table: "SanPhamChiTiets",
                column: "IdChatLieu",
                principalTable: "ChatLieus",
                principalColumn: "IdChatLieu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiets_KichCos_IdKichCo",
                table: "SanPhamChiTiets",
                column: "IdKichCo",
                principalTable: "KichCos",
                principalColumn: "IdKichCo");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiets_KieuDeGiays_IdKieuDeGiay",
                table: "SanPhamChiTiets",
                column: "IdKieuDeGiay",
                principalTable: "KieuDeGiays",
                principalColumn: "IdKieuDeGiay");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiets_LoaiGiays_IdLoaiGiay",
                table: "SanPhamChiTiets",
                column: "IdLoaiGiay",
                principalTable: "LoaiGiays",
                principalColumn: "IdLoaiGiay");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiets_MauSacs_IdMauSac",
                table: "SanPhamChiTiets",
                column: "IdMauSac",
                principalTable: "MauSacs",
                principalColumn: "IdMauSac");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiets_SanPhams_IdSanPham",
                table: "SanPhamChiTiets",
                column: "IdSanPham",
                principalTable: "SanPhams",
                principalColumn: "IdSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiets_ThuongHieus_IdThuongHieu",
                table: "SanPhamChiTiets",
                column: "IdThuongHieu",
                principalTable: "ThuongHieus",
                principalColumn: "IdThuongHieu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiets_XuatXus_IdXuatXu",
                table: "SanPhamChiTiets",
                column: "IdXuatXu",
                principalTable: "XuatXus",
                principalColumn: "IdXuatXu");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamYeuThichs_SanPhamChiTiets_IdSanPhamChiTiet",
                table: "SanPhamYeuThichs",
                column: "IdSanPhamChiTiet",
                principalTable: "SanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamYeuThichs_Users_IdNguoiDung",
                table: "SanPhamYeuThichs",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ThongTinGiaoHangs_Users_IdNguoiDung",
                table: "ThongTinGiaoHangs",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClaims_Users_UserId",
                table: "UserClaims",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLogins_Users_UserId",
                table: "UserLogins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Roles_RoleId",
                table: "UserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoles_Users_UserId",
                table: "UserRoles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTokens_Users_UserId",
                table: "UserTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherNguoiDungs_Users_IdNguoiDung",
                table: "VoucherNguoiDungs",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VoucherNguoiDungs_Vouchers_IdVouCher",
                table: "VoucherNguoiDungs",
                column: "IdVouCher",
                principalTable: "Vouchers",
                principalColumn: "IdVoucher");
        }
    }
}

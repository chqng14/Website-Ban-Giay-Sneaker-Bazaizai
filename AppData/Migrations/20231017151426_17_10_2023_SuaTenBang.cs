using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class _17_10_2023_SuaTenBang : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "Anh");

            migrationBuilder.DropForeignKey(
                name: "FK_Đánh giá_Đánh giá_ParentId",
                table: "Đánh giá");

            migrationBuilder.DropForeignKey(
                name: "FK_Đánh giá_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "Đánh giá");

            migrationBuilder.DropForeignKey(
                name: "FK_Đánh giá_Users_IdNguoiDung",
                table: "Đánh giá");

            migrationBuilder.DropForeignKey(
                name: "FK_gioHangChiTiets_gioHangs_IdNguoiDung",
                table: "gioHangChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_gioHangChiTiets_sanPhamChiTiets_IdSanPhamCT",
                table: "gioHangChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_gioHangs_Users_NguoiDungId",
                table: "gioHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_hoaDonChiTiets_HoaDons_IdHoaDon",
                table: "hoaDonChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_hoaDonChiTiets_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "hoaDonChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhachHang_IdKhachHang",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_thongTinGiaoHangs_IdThongTinGH",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_Users_IdNguoiDung",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_vouchers_IdVoucher",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_Users_IdNguoiDung",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                table: "phuongThucThanhToanChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                table: "phuongThucThanhToanChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleClaims_Roles_RoleId",
                table: "RoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamChiTiets_ChatLieus_IdChatLieu",
                table: "sanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamChiTiets_kichCos_IdKichCo",
                table: "sanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamChiTiets_kieuDeGiays_IdKieuDeGiay",
                table: "sanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamChiTiets_LoaiGiays_IdLoaiGiay",
                table: "sanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamChiTiets_mauSacs_IdMauSac",
                table: "sanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamChiTiets_SanPhams_IdSanPham",
                table: "sanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamChiTiets_thuongHieus_IdThuongHieu",
                table: "sanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamChiTiets_xuatXus_IdXuatXu",
                table: "sanPhamChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamYeuThiches_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamYeuThiches_Users_IdNguoiDung",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropForeignKey(
                name: "FK_thongTinGiaoHangs_Users_IdNguoiDung",
                table: "thongTinGiaoHangs");

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
                name: "FK_voucherNguoiDungs_Users_IdNguoiDung",
                table: "voucherNguoiDungs");

            migrationBuilder.DropForeignKey(
                name: "FK_voucherNguoiDungs_vouchers_IdVouCher",
                table: "voucherNguoiDungs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_xuatXus",
                table: "xuatXus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_vouchers",
                table: "vouchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_voucherNguoiDungs",
                table: "voucherNguoiDungs");

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
                name: "PK_thuongHieus",
                table: "thuongHieus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_thongTinGiaoHangs",
                table: "thongTinGiaoHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sanPhamYeuThiches",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SanPhams",
                table: "SanPhams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sanPhamChiTiets",
                table: "sanPhamChiTiets");

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
                name: "PK_phuongThucThanhToanChiTiets",
                table: "phuongThucThanhToanChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_mauSacs",
                table: "mauSacs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LoaiGiays",
                table: "LoaiGiays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kieuDeGiays",
                table: "kieuDeGiays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kichCos",
                table: "kichCos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_HoaDons",
                table: "HoaDons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_hoaDonChiTiets",
                table: "hoaDonChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gioHangs",
                table: "gioHangs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gioHangChiTiets",
                table: "gioHangChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Đánh giá",
                table: "Đánh giá");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatLieus",
                table: "ChatLieus");

            migrationBuilder.RenameTable(
                name: "xuatXus",
                newName: "XuatXu");

            migrationBuilder.RenameTable(
                name: "vouchers",
                newName: "Voucher");

            migrationBuilder.RenameTable(
                name: "voucherNguoiDungs",
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
                name: "thuongHieus",
                newName: "ThuongHieu");

            migrationBuilder.RenameTable(
                name: "thongTinGiaoHangs",
                newName: "ThongTinGiaoHang");

            migrationBuilder.RenameTable(
                name: "sanPhamYeuThiches",
                newName: "SanPhamYeuThich");

            migrationBuilder.RenameTable(
                name: "SanPhams",
                newName: "SanPham");

            migrationBuilder.RenameTable(
                name: "sanPhamChiTiets",
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
                name: "phuongThucThanhToanChiTiets",
                newName: "PhuongThucThanhToanChiTiet");

            migrationBuilder.RenameTable(
                name: "mauSacs",
                newName: "MauSac");

            migrationBuilder.RenameTable(
                name: "LoaiGiays",
                newName: "LoaiGiay");

            migrationBuilder.RenameTable(
                name: "kieuDeGiays",
                newName: "KieuDeGiay");

            migrationBuilder.RenameTable(
                name: "kichCos",
                newName: "KichCo");

            migrationBuilder.RenameTable(
                name: "HoaDons",
                newName: "HoaDon");

            migrationBuilder.RenameTable(
                name: "hoaDonChiTiets",
                newName: "HoaDonChiTiet");

            migrationBuilder.RenameTable(
                name: "gioHangs",
                newName: "GioHang");

            migrationBuilder.RenameTable(
                name: "gioHangChiTiets",
                newName: "GioHangChiTiet");

            migrationBuilder.RenameTable(
                name: "Đánh giá",
                newName: "DanhGia");

            migrationBuilder.RenameTable(
                name: "ChatLieus",
                newName: "ChatLieu");

            migrationBuilder.RenameIndex(
                name: "IX_voucherNguoiDungs_IdVouCher",
                table: "VoucherNguoiDung",
                newName: "IX_VoucherNguoiDung_IdVouCher");

            migrationBuilder.RenameIndex(
                name: "IX_voucherNguoiDungs_IdNguoiDung",
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
                name: "IX_thongTinGiaoHangs_IdNguoiDung",
                table: "ThongTinGiaoHang",
                newName: "IX_ThongTinGiaoHang_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamYeuThiches_IdSanPhamChiTiet",
                table: "SanPhamYeuThich",
                newName: "IX_SanPhamYeuThich_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamYeuThiches_IdNguoiDung",
                table: "SanPhamYeuThich",
                newName: "IX_SanPhamYeuThich_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamChiTiets_IdXuatXu",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdXuatXu");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamChiTiets_IdThuongHieu",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdThuongHieu");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamChiTiets_IdSanPham",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdSanPham");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamChiTiets_IdMauSac",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdMauSac");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamChiTiets_IdLoaiGiay",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdLoaiGiay");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamChiTiets_IdKieuDeGiay",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdKieuDeGiay");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamChiTiets_IdKichCo",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdKichCo");

            migrationBuilder.RenameIndex(
                name: "IX_sanPhamChiTiets_IdChatLieu",
                table: "SanPhamChiTiet",
                newName: "IX_SanPhamChiTiet_IdChatLieu");

            migrationBuilder.RenameIndex(
                name: "IX_RoleClaims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_phuongThucThanhToanChiTiets_IdThanhToan",
                table: "PhuongThucThanhToanChiTiet",
                newName: "IX_PhuongThucThanhToanChiTiet_IdThanhToan");

            migrationBuilder.RenameIndex(
                name: "IX_phuongThucThanhToanChiTiets_IdHoaDon",
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
                name: "IX_hoaDonChiTiets_IdSanPhamChiTiet",
                table: "HoaDonChiTiet",
                newName: "IX_HoaDonChiTiet_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_hoaDonChiTiets_IdHoaDon",
                table: "HoaDonChiTiet",
                newName: "IX_HoaDonChiTiet_IdHoaDon");

            migrationBuilder.RenameIndex(
                name: "IX_gioHangs_NguoiDungId",
                table: "GioHang",
                newName: "IX_GioHang_NguoiDungId");

            migrationBuilder.RenameIndex(
                name: "IX_gioHangChiTiets_IdSanPhamCT",
                table: "GioHangChiTiet",
                newName: "IX_GioHangChiTiet_IdSanPhamCT");

            migrationBuilder.RenameIndex(
                name: "IX_gioHangChiTiets_IdNguoiDung",
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

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGia_DanhGia_ParentId",
                table: "DanhGia",
                column: "ParentId",
                principalTable: "DanhGia",
                principalColumn: "IdDanhGia");

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

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGia_DanhGia_ParentId",
                table: "DanhGia");

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
                newName: "xuatXus");

            migrationBuilder.RenameTable(
                name: "VoucherNguoiDung",
                newName: "voucherNguoiDungs");

            migrationBuilder.RenameTable(
                name: "Voucher",
                newName: "vouchers");

            migrationBuilder.RenameTable(
                name: "ThuongHieu",
                newName: "thuongHieus");

            migrationBuilder.RenameTable(
                name: "ThongTinGiaoHang",
                newName: "thongTinGiaoHangs");

            migrationBuilder.RenameTable(
                name: "SanPhamYeuThich",
                newName: "sanPhamYeuThiches");

            migrationBuilder.RenameTable(
                name: "SanPhamChiTiet",
                newName: "sanPhamChiTiets");

            migrationBuilder.RenameTable(
                name: "SanPham",
                newName: "SanPhams");

            migrationBuilder.RenameTable(
                name: "PhuongThucThanhToanChiTiet",
                newName: "phuongThucThanhToanChiTiets");

            migrationBuilder.RenameTable(
                name: "PhuongThucThanhToan",
                newName: "PhuongThucThanhToans");

            migrationBuilder.RenameTable(
                name: "NguoiDung",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "MauSac",
                newName: "mauSacs");

            migrationBuilder.RenameTable(
                name: "LoaiGiay",
                newName: "LoaiGiays");

            migrationBuilder.RenameTable(
                name: "KieuDeGiay",
                newName: "kieuDeGiays");

            migrationBuilder.RenameTable(
                name: "KichCo",
                newName: "kichCos");

            migrationBuilder.RenameTable(
                name: "HoaDonChiTiet",
                newName: "hoaDonChiTiets");

            migrationBuilder.RenameTable(
                name: "HoaDon",
                newName: "HoaDons");

            migrationBuilder.RenameTable(
                name: "GioHangChiTiet",
                newName: "gioHangChiTiets");

            migrationBuilder.RenameTable(
                name: "GioHang",
                newName: "gioHangs");

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
                table: "voucherNguoiDungs",
                newName: "IX_voucherNguoiDungs_IdVouCher");

            migrationBuilder.RenameIndex(
                name: "IX_VoucherNguoiDung_IdNguoiDung",
                table: "voucherNguoiDungs",
                newName: "IX_voucherNguoiDungs_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_ThongTinGiaoHang_IdNguoiDung",
                table: "thongTinGiaoHangs",
                newName: "IX_thongTinGiaoHangs_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamYeuThich_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                newName: "IX_sanPhamYeuThiches_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamYeuThich_IdNguoiDung",
                table: "sanPhamYeuThiches",
                newName: "IX_sanPhamYeuThiches_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdXuatXu",
                table: "sanPhamChiTiets",
                newName: "IX_sanPhamChiTiets_IdXuatXu");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdThuongHieu",
                table: "sanPhamChiTiets",
                newName: "IX_sanPhamChiTiets_IdThuongHieu");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdSanPham",
                table: "sanPhamChiTiets",
                newName: "IX_sanPhamChiTiets_IdSanPham");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdMauSac",
                table: "sanPhamChiTiets",
                newName: "IX_sanPhamChiTiets_IdMauSac");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdLoaiGiay",
                table: "sanPhamChiTiets",
                newName: "IX_sanPhamChiTiets_IdLoaiGiay");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdKieuDeGiay",
                table: "sanPhamChiTiets",
                newName: "IX_sanPhamChiTiets_IdKieuDeGiay");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdKichCo",
                table: "sanPhamChiTiets",
                newName: "IX_sanPhamChiTiets_IdKichCo");

            migrationBuilder.RenameIndex(
                name: "IX_SanPhamChiTiet_IdChatLieu",
                table: "sanPhamChiTiets",
                newName: "IX_sanPhamChiTiets_IdChatLieu");

            migrationBuilder.RenameIndex(
                name: "IX_PhuongThucThanhToanChiTiet_IdThanhToan",
                table: "phuongThucThanhToanChiTiets",
                newName: "IX_phuongThucThanhToanChiTiets_IdThanhToan");

            migrationBuilder.RenameIndex(
                name: "IX_PhuongThucThanhToanChiTiet_IdHoaDon",
                table: "phuongThucThanhToanChiTiets",
                newName: "IX_phuongThucThanhToanChiTiets_IdHoaDon");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDonChiTiet_IdSanPhamChiTiet",
                table: "hoaDonChiTiets",
                newName: "IX_hoaDonChiTiets_IdSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_HoaDonChiTiet_IdHoaDon",
                table: "hoaDonChiTiets",
                newName: "IX_hoaDonChiTiets_IdHoaDon");

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
                table: "gioHangChiTiets",
                newName: "IX_gioHangChiTiets_IdSanPhamCT");

            migrationBuilder.RenameIndex(
                name: "IX_GioHangChiTiet_IdNguoiDung",
                table: "gioHangChiTiets",
                newName: "IX_gioHangChiTiets_IdNguoiDung");

            migrationBuilder.RenameIndex(
                name: "IX_GioHang_NguoiDungId",
                table: "gioHangs",
                newName: "IX_gioHangs_NguoiDungId");

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
                name: "PK_xuatXus",
                table: "xuatXus",
                column: "IdXuatXu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_voucherNguoiDungs",
                table: "voucherNguoiDungs",
                column: "IdVouCherNguoiDung");

            migrationBuilder.AddPrimaryKey(
                name: "PK_vouchers",
                table: "vouchers",
                column: "IdVoucher");

            migrationBuilder.AddPrimaryKey(
                name: "PK_thuongHieus",
                table: "thuongHieus",
                column: "IdThuongHieu");

            migrationBuilder.AddPrimaryKey(
                name: "PK_thongTinGiaoHangs",
                table: "thongTinGiaoHangs",
                column: "IdThongTinGH");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sanPhamYeuThiches",
                table: "sanPhamYeuThiches",
                column: "IdSanPhamYeuThich");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sanPhamChiTiets",
                table: "sanPhamChiTiets",
                column: "IdChiTietSp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SanPhams",
                table: "SanPhams",
                column: "IdSanPham");

            migrationBuilder.AddPrimaryKey(
                name: "PK_phuongThucThanhToanChiTiets",
                table: "phuongThucThanhToanChiTiets",
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
                name: "PK_mauSacs",
                table: "mauSacs",
                column: "IdMauSac");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoaiGiays",
                table: "LoaiGiays",
                column: "IdLoaiGiay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_kieuDeGiays",
                table: "kieuDeGiays",
                column: "IdKieuDeGiay");

            migrationBuilder.AddPrimaryKey(
                name: "PK_kichCos",
                table: "kichCos",
                column: "IdKichCo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_hoaDonChiTiets",
                table: "hoaDonChiTiets",
                column: "IdHoaDonChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HoaDons",
                table: "HoaDons",
                column: "IdHoaDon");

            migrationBuilder.AddPrimaryKey(
                name: "PK_gioHangChiTiets",
                table: "gioHangChiTiets",
                column: "IdGioHangChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_gioHangs",
                table: "gioHangs",
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
                name: "FK_Anh_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "Anh",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_Đánh giá_Đánh giá_ParentId",
                table: "Đánh giá",
                column: "ParentId",
                principalTable: "Đánh giá",
                principalColumn: "IdDanhGia");

            migrationBuilder.AddForeignKey(
                name: "FK_Đánh giá_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "Đánh giá",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_Đánh giá_Users_IdNguoiDung",
                table: "Đánh giá",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_gioHangChiTiets_gioHangs_IdNguoiDung",
                table: "gioHangChiTiets",
                column: "IdNguoiDung",
                principalTable: "gioHangs",
                principalColumn: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_gioHangChiTiets_sanPhamChiTiets_IdSanPhamCT",
                table: "gioHangChiTiets",
                column: "IdSanPhamCT",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_gioHangs_Users_NguoiDungId",
                table: "gioHangs",
                column: "NguoiDungId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_hoaDonChiTiets_HoaDons_IdHoaDon",
                table: "hoaDonChiTiets",
                column: "IdHoaDon",
                principalTable: "HoaDons",
                principalColumn: "IdHoaDon");

            migrationBuilder.AddForeignKey(
                name: "FK_hoaDonChiTiets_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "hoaDonChiTiets",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhachHang_IdKhachHang",
                table: "HoaDons",
                column: "IdKhachHang",
                principalTable: "KhachHang",
                principalColumn: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_thongTinGiaoHangs_IdThongTinGH",
                table: "HoaDons",
                column: "IdThongTinGH",
                principalTable: "thongTinGiaoHangs",
                principalColumn: "IdThongTinGH");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_Users_IdNguoiDung",
                table: "HoaDons",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_vouchers_IdVoucher",
                table: "HoaDons",
                column: "IdVoucher",
                principalTable: "vouchers",
                principalColumn: "IdVoucher");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_Users_IdNguoiDung",
                table: "KhachHang",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                table: "phuongThucThanhToanChiTiets",
                column: "IdHoaDon",
                principalTable: "HoaDons",
                principalColumn: "IdHoaDon");

            migrationBuilder.AddForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                table: "phuongThucThanhToanChiTiets",
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
                name: "FK_sanPhamChiTiets_ChatLieus_IdChatLieu",
                table: "sanPhamChiTiets",
                column: "IdChatLieu",
                principalTable: "ChatLieus",
                principalColumn: "IdChatLieu");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamChiTiets_kichCos_IdKichCo",
                table: "sanPhamChiTiets",
                column: "IdKichCo",
                principalTable: "kichCos",
                principalColumn: "IdKichCo");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamChiTiets_kieuDeGiays_IdKieuDeGiay",
                table: "sanPhamChiTiets",
                column: "IdKieuDeGiay",
                principalTable: "kieuDeGiays",
                principalColumn: "IdKieuDeGiay");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamChiTiets_LoaiGiays_IdLoaiGiay",
                table: "sanPhamChiTiets",
                column: "IdLoaiGiay",
                principalTable: "LoaiGiays",
                principalColumn: "IdLoaiGiay");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamChiTiets_mauSacs_IdMauSac",
                table: "sanPhamChiTiets",
                column: "IdMauSac",
                principalTable: "mauSacs",
                principalColumn: "IdMauSac");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamChiTiets_SanPhams_IdSanPham",
                table: "sanPhamChiTiets",
                column: "IdSanPham",
                principalTable: "SanPhams",
                principalColumn: "IdSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamChiTiets_thuongHieus_IdThuongHieu",
                table: "sanPhamChiTiets",
                column: "IdThuongHieu",
                principalTable: "thuongHieus",
                principalColumn: "IdThuongHieu");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamChiTiets_xuatXus_IdXuatXu",
                table: "sanPhamChiTiets",
                column: "IdXuatXu",
                principalTable: "xuatXus",
                principalColumn: "IdXuatXu");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamYeuThiches_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamYeuThiches_Users_IdNguoiDung",
                table: "sanPhamYeuThiches",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_thongTinGiaoHangs_Users_IdNguoiDung",
                table: "thongTinGiaoHangs",
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
                name: "FK_voucherNguoiDungs_Users_IdNguoiDung",
                table: "voucherNguoiDungs",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_voucherNguoiDungs_vouchers_IdVouCher",
                table: "voucherNguoiDungs",
                column: "IdVouCher",
                principalTable: "vouchers",
                principalColumn: "IdVoucher");
        }
    }
}

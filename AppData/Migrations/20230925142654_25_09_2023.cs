using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class _25_09_2023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "Anh");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_NguoiDungs_IdNguoiDung",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_KhuyenMai_IdKhuyenMai",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungs_chucVus_IdChucVu",
                table: "NguoiDungs");

            migrationBuilder.DropForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                table: "phuongThucThanhToanChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                table: "phuongThucThanhToanChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamYeuThiches_NguoiDungs_IdNguoiDung",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamYeuThiches_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropForeignKey(
                name: "FK_thongTinGiaoHangs_NguoiDungs_IdNguoiDung",
                table: "thongTinGiaoHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_voucherNguoiDungs_NguoiDungs_IdNguoiDung",
                table: "voucherNguoiDungs");

            migrationBuilder.DropForeignKey(
                name: "FK_voucherNguoiDungs_vouchers_IdVouCher",
                table: "voucherNguoiDungs");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "xuatXus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "xuatXus",
                type: "nvarchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)");

            migrationBuilder.AlterColumn<string>(
                name: "Ma",
                table: "xuatXus",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "vouchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "voucherNguoiDungs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "IdVouCher",
                table: "voucherNguoiDungs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdNguoiDung",
                table: "voucherNguoiDungs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "thuongHieus",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "MaThuongHieu",
                table: "thuongHieus",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "thongTinGiaoHangs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "IdNguoiDung",
                table: "thongTinGiaoHangs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "sanPhamYeuThiches",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdNguoiDung",
                table: "sanPhamYeuThiches",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Trangthai",
                table: "SanPhams",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TenSanPham",
                table: "SanPhams",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<string>(
                name: "MaSanPham",
                table: "SanPhams",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "PhuongThucThanhToans",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TenPhuongThucThanhToan",
                table: "PhuongThucThanhToans",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<string>(
                name: "MaPhuongThucThanhToan",
                table: "PhuongThucThanhToans",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<double>(
                name: "SoTien",
                table: "phuongThucThanhToanChiTiets",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "IdThanhToan",
                table: "phuongThucThanhToanChiTiets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdHoaDon",
                table: "phuongThucThanhToanChiTiets",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdChucVu",
                table: "NguoiDungs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "mauSacs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TenMauSac",
                table: "mauSacs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "MaMauSac",
                table: "mauSacs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Trangthai",
                table: "kieuDeGiays",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TenKieuDeGiay",
                table: "kieuDeGiays",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<string>(
                name: "MaKieuDeGiay",
                table: "kieuDeGiays",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "kichCos",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "KhuyenMaiChiTiet",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "KhuyenMaiChiTiet",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdKhuyenMai",
                table: "KhuyenMaiChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "KhuyenMai",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "TenKhuyenMai",
                table: "KhuyenMai",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayKetThuc",
                table: "KhuyenMai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayBatDau",
                table: "KhuyenMai",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<decimal>(
                name: "MucGiam",
                table: "KhuyenMai",
                type: "decimal(18,0)",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "MaKhuyenMai",
                table: "KhuyenMai",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "LoaiHinhKM",
                table: "KhuyenMai",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "IdNguoiDung",
                table: "KhachHang",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "gioHangs",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayTao",
                table: "gioHangs",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "gioHangChiTiets",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "ChatLieus",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TenChatLieu",
                table: "ChatLieus",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<string>(
                name: "MaChatLieu",
                table: "ChatLieus",
                type: "nvarchar(1000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "Anh",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPhamChiTiet",
                table: "Anh",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "Anh",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_NguoiDungs_IdNguoiDung",
                table: "KhachHang",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_KhuyenMai_IdKhuyenMai",
                table: "KhuyenMaiChiTiet",
                column: "IdKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "IdKhuyenMai");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungs_chucVus_IdChucVu",
                table: "NguoiDungs",
                column: "IdChucVu",
                principalTable: "chucVus",
                principalColumn: "IdChucVu");

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
                name: "FK_sanPhamYeuThiches_NguoiDungs_IdNguoiDung",
                table: "sanPhamYeuThiches",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamYeuThiches_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp");

            migrationBuilder.AddForeignKey(
                name: "FK_thongTinGiaoHangs_NguoiDungs_IdNguoiDung",
                table: "thongTinGiaoHangs",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_voucherNguoiDungs_NguoiDungs_IdNguoiDung",
                table: "voucherNguoiDungs",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_voucherNguoiDungs_vouchers_IdVouCher",
                table: "voucherNguoiDungs",
                column: "IdVouCher",
                principalTable: "vouchers",
                principalColumn: "IdVoucher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "Anh");

            migrationBuilder.DropForeignKey(
                name: "FK_KhachHang_NguoiDungs_IdNguoiDung",
                table: "KhachHang");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_KhuyenMai_IdKhuyenMai",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungs_chucVus_IdChucVu",
                table: "NguoiDungs");

            migrationBuilder.DropForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                table: "phuongThucThanhToanChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                table: "phuongThucThanhToanChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamYeuThiches_NguoiDungs_IdNguoiDung",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamYeuThiches_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropForeignKey(
                name: "FK_thongTinGiaoHangs_NguoiDungs_IdNguoiDung",
                table: "thongTinGiaoHangs");

            migrationBuilder.DropForeignKey(
                name: "FK_voucherNguoiDungs_NguoiDungs_IdNguoiDung",
                table: "voucherNguoiDungs");

            migrationBuilder.DropForeignKey(
                name: "FK_voucherNguoiDungs_vouchers_IdVouCher",
                table: "voucherNguoiDungs");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "xuatXus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "xuatXus",
                type: "nvarchar(250)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ma",
                table: "xuatXus",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "voucherNguoiDungs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdVouCher",
                table: "voucherNguoiDungs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdNguoiDung",
                table: "voucherNguoiDungs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "thuongHieus",
                type: "int",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "MaThuongHieu",
                table: "thuongHieus",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "thongTinGiaoHangs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdNguoiDung",
                table: "thongTinGiaoHangs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "sanPhamYeuThiches",
                type: "int",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdNguoiDung",
                table: "sanPhamYeuThiches",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Trangthai",
                table: "SanPhams",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenSanPham",
                table: "SanPhams",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaSanPham",
                table: "SanPhams",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "PhuongThucThanhToans",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenPhuongThucThanhToan",
                table: "PhuongThucThanhToans",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaPhuongThucThanhToan",
                table: "PhuongThucThanhToans",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "SoTien",
                table: "phuongThucThanhToanChiTiets",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdThanhToan",
                table: "phuongThucThanhToanChiTiets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdHoaDon",
                table: "phuongThucThanhToanChiTiets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdChucVu",
                table: "NguoiDungs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "mauSacs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenMauSac",
                table: "mauSacs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaMauSac",
                table: "mauSacs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Trangthai",
                table: "kieuDeGiays",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenKieuDeGiay",
                table: "kieuDeGiays",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaKieuDeGiay",
                table: "kieuDeGiays",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "kichCos",
                type: "int",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "KhuyenMaiChiTiet",
                type: "int",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "KhuyenMaiChiTiet",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdKhuyenMai",
                table: "KhuyenMaiChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "KhuyenMai",
                type: "int",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "TenKhuyenMai",
                table: "KhuyenMai",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayKetThuc",
                table: "KhuyenMai",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayBatDau",
                table: "KhuyenMai",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "MucGiam",
                table: "KhuyenMai",
                type: "decimal(18,0)",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "MaKhuyenMai",
                table: "KhuyenMai",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoaiHinhKM",
                table: "KhuyenMai",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdNguoiDung",
                table: "KhachHang",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "gioHangs",
                type: "int",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgayTao",
                table: "gioHangs",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "gioHangChiTiets",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "ChatLieus",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TenChatLieu",
                table: "ChatLieus",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaChatLieu",
                table: "ChatLieus",
                type: "nvarchar(1000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "Anh",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPhamChiTiet",
                table: "Anh",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "Anh",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KhachHang_NguoiDungs_IdNguoiDung",
                table: "KhachHang",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_KhuyenMai_IdKhuyenMai",
                table: "KhuyenMaiChiTiet",
                column: "IdKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "IdKhuyenMai",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungs_chucVus_IdChucVu",
                table: "NguoiDungs",
                column: "IdChucVu",
                principalTable: "chucVus",
                principalColumn: "IdChucVu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                table: "phuongThucThanhToanChiTiets",
                column: "IdHoaDon",
                principalTable: "HoaDons",
                principalColumn: "IdHoaDon",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_phuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                table: "phuongThucThanhToanChiTiets",
                column: "IdThanhToan",
                principalTable: "PhuongThucThanhToans",
                principalColumn: "IdPhuongThucThanhToan",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamYeuThiches_NguoiDungs_IdNguoiDung",
                table: "sanPhamYeuThiches",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamYeuThiches_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_thongTinGiaoHangs_NguoiDungs_IdNguoiDung",
                table: "thongTinGiaoHangs",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_voucherNguoiDungs_NguoiDungs_IdNguoiDung",
                table: "voucherNguoiDungs",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_voucherNguoiDungs_vouchers_IdVouCher",
                table: "voucherNguoiDungs",
                column: "IdVouCher",
                principalTable: "vouchers",
                principalColumn: "IdVoucher",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

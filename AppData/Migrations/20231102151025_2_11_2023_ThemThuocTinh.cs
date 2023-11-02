using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class _2_11_2023_ThemThuocTinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrangThai",
                table: "ThongTinGiaoHang",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuaDoi",
                table: "NguoiDung",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TongChiTieu",
                table: "NguoiDung",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatLuongSanPham",
                table: "DanhGia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LuotYeuThich",
                table: "DanhGia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MoTa",
                table: "DanhGia",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuaDoi",
                table: "DanhGia",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "Anh",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrangThai",
                table: "ThongTinGiaoHang");

            migrationBuilder.DropColumn(
                name: "SuaDoi",
                table: "NguoiDung");

            migrationBuilder.DropColumn(
                name: "TongChiTieu",
                table: "NguoiDung");

            migrationBuilder.DropColumn(
                name: "ChatLuongSanPham",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "LuotYeuThich",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "MoTa",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "SuaDoi",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "Anh");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class _26_9_lan1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "sanPhamChiTiets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "Nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayTao",
                table: "sanPhamChiTiets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NoiBat",
                table: "sanPhamChiTiets",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoLuongDaBan",
                table: "sanPhamChiTiets",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayTao",
                table: "sanPhamChiTiets");

            migrationBuilder.DropColumn(
                name: "NoiBat",
                table: "sanPhamChiTiets");

            migrationBuilder.DropColumn(
                name: "SoLuongDaBan",
                table: "sanPhamChiTiets");

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "sanPhamChiTiets",
                type: "Nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class _3_10_2023_lan1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhachHang_IdNguoiDung",
                table: "HoaDons");

            migrationBuilder.AlterColumn<string>(
                name: "IdKhachHang",
                table: "HoaDons",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IdKhachHang",
                table: "HoaDons",
                column: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhachHang_IdKhachHang",
                table: "HoaDons",
                column: "IdKhachHang",
                principalTable: "KhachHang",
                principalColumn: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_Users_IdNguoiDung",
                table: "HoaDons",
                column: "IdNguoiDung",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhachHang_IdKhachHang",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_Users_IdNguoiDung",
                table: "HoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDons_IdKhachHang",
                table: "HoaDons");

            migrationBuilder.AlterColumn<string>(
                name: "IdKhachHang",
                table: "HoaDons",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhachHang_IdNguoiDung",
                table: "HoaDons",
                column: "IdNguoiDung",
                principalTable: "KhachHang",
                principalColumn: "IdKhachHang");
        }
    }
}

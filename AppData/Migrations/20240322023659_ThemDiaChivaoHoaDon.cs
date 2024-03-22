using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class ThemDiaChivaoHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AlterColumn<string>(
                name: "IdXuatXu",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdThuongHieu",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPham",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdMauSac",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdLoaiGiay",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdKieuDeGiay",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdKichCo",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IdChatLieu",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "KhuyenMai",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
                name: "MaKhuyenMai",
                table: "KhuyenMai",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_ChatLieu_IdChatLieu",
                table: "SanPhamChiTiet",
                column: "IdChatLieu",
                principalTable: "ChatLieu",
                principalColumn: "IdChatLieu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_KichCo_IdKichCo",
                table: "SanPhamChiTiet",
                column: "IdKichCo",
                principalTable: "KichCo",
                principalColumn: "IdKichCo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_KieuDeGiay_IdKieuDeGiay",
                table: "SanPhamChiTiet",
                column: "IdKieuDeGiay",
                principalTable: "KieuDeGiay",
                principalColumn: "IdKieuDeGiay",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_LoaiGiay_IdLoaiGiay",
                table: "SanPhamChiTiet",
                column: "IdLoaiGiay",
                principalTable: "LoaiGiay",
                principalColumn: "IdLoaiGiay",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_MauSac_IdMauSac",
                table: "SanPhamChiTiet",
                column: "IdMauSac",
                principalTable: "MauSac",
                principalColumn: "IdMauSac",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_SanPham_IdSanPham",
                table: "SanPhamChiTiet",
                column: "IdSanPham",
                principalTable: "SanPham",
                principalColumn: "IdSanPham",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_ThuongHieu_IdThuongHieu",
                table: "SanPhamChiTiet",
                column: "IdThuongHieu",
                principalTable: "ThuongHieu",
                principalColumn: "IdThuongHieu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPhamChiTiet_XuatXu_IdXuatXu",
                table: "SanPhamChiTiet",
                column: "IdXuatXu",
                principalTable: "XuatXu",
                principalColumn: "IdXuatXu",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "HoaDon");

            migrationBuilder.AlterColumn<string>(
                name: "IdXuatXu",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdThuongHieu",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPham",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdMauSac",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdLoaiGiay",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdKieuDeGiay",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdKichCo",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdChatLieu",
                table: "SanPhamChiTiet",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "KhuyenMai",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
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
                name: "MaKhuyenMai",
                table: "KhuyenMai",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

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
        }
    }
}

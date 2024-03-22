using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class thembangdanhgia16_10_2023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "Users",
                type: "nvarchar(300)",
                nullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "PhamVi",
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
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LoaiHinhKM",
                table: "KhuyenMai",
                type: "int",
                maxLength: 1000,
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Đánh giá",
                columns: table => new
                {
                    IdDanhGia = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "(newid())"),
                    BinhLuan = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SaoSp = table.Column<int>(type: "int", nullable: true),
                    SaoVanChuyen = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Đánh giá", x => x.IdDanhGia);
                    table.ForeignKey(
                        name: "FK_Đánh giá_Đánh giá_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Đánh giá",
                        principalColumn: "IdDanhGia");
                    table.ForeignKey(
                        name: "FK_Đánh giá_SanPhamChiTiets_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiets",
                        principalColumn: "IdChiTietSp");
                    table.ForeignKey(
                        name: "FK_Đánh giá_Users_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Đánh giá_IdNguoiDung",
                table: "Đánh giá",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_Đánh giá_IdSanPhamChiTiet",
                table: "Đánh giá",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_Đánh giá_ParentId",
                table: "Đánh giá",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Đánh giá");

            migrationBuilder.DropColumn(
                name: "DiaChi",
                table: "Users");

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

            migrationBuilder.AlterColumn<string>(
                name: "PhamVi",
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
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<int>(
                name: "LoaiHinhKM",
                table: "KhuyenMai",
                type: "int",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 1000);
        }
    }
}

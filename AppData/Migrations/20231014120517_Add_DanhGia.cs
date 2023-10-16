using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class Add_DanhGia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaChi",
                table: "Users",
                type: "nvarchar(300)",
                nullable: true);

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
                        name: "FK_Đánh giá_sanPhamChiTiets_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "sanPhamChiTiets",
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
        }
    }
}

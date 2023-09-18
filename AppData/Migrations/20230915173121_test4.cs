using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class test4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NguoiDungThongTinGiaoHang");

            migrationBuilder.CreateIndex(
                name: "IX_thongTinGiaoHangs_IdNguoiDung",
                table: "thongTinGiaoHangs",
                column: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_thongTinGiaoHangs_NguoiDungs_IdNguoiDung",
                table: "thongTinGiaoHangs",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_thongTinGiaoHangs_NguoiDungs_IdNguoiDung",
                table: "thongTinGiaoHangs");

            migrationBuilder.DropIndex(
                name: "IX_thongTinGiaoHangs_IdNguoiDung",
                table: "thongTinGiaoHangs");

            migrationBuilder.CreateTable(
                name: "NguoiDungThongTinGiaoHang",
                columns: table => new
                {
                    NguoiDungsIdNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ThongTinGiaoHangsIdThongTinGH = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungThongTinGiaoHang", x => new { x.NguoiDungsIdNguoiDung, x.ThongTinGiaoHangsIdThongTinGH });
                    table.ForeignKey(
                        name: "FK_NguoiDungThongTinGiaoHang_NguoiDungs_NguoiDungsIdNguoiDung",
                        column: x => x.NguoiDungsIdNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "IdNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NguoiDungThongTinGiaoHang_thongTinGiaoHangs_ThongTinGiaoHangsIdThongTinGH",
                        column: x => x.ThongTinGiaoHangsIdThongTinGH,
                        principalTable: "thongTinGiaoHangs",
                        principalColumn: "IdThongTinGH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungThongTinGiaoHang_ThongTinGiaoHangsIdThongTinGH",
                table: "NguoiDungThongTinGiaoHang",
                column: "ThongTinGiaoHangsIdThongTinGH");
        }
    }
}

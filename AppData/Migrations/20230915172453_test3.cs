using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class test3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_khuyenMaiChiTiets_khuyenMais_IDKhuyenMai",
                table: "khuyenMaiChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_khuyenMaiChiTiets_sanPhamChiTiets_IDSanPhamChiTiet",
                table: "khuyenMaiChiTiets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_khuyenMais",
                table: "khuyenMais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_khuyenMaiChiTiets",
                table: "khuyenMaiChiTiets");

            migrationBuilder.RenameTable(
                name: "khuyenMais",
                newName: "KhuyenMai");

            migrationBuilder.RenameTable(
                name: "khuyenMaiChiTiets",
                newName: "KhuyenMaiChiTiet");

            migrationBuilder.RenameColumn(
                name: "Ten",
                table: "mauSacs",
                newName: "TenMauSac");

            migrationBuilder.RenameColumn(
                name: "Ma",
                table: "mauSacs",
                newName: "MaMauSac");

            migrationBuilder.RenameIndex(
                name: "IX_khuyenMaiChiTiets_IDSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                newName: "IX_KhuyenMaiChiTiet_IDSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_khuyenMaiChiTiets_IDKhuyenMai",
                table: "KhuyenMaiChiTiet",
                newName: "IX_KhuyenMaiChiTiet_IDKhuyenMai");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhuyenMai",
                table: "KhuyenMai",
                column: "IDKhuyenMai");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KhuyenMaiChiTiet",
                table: "KhuyenMaiChiTiet",
                column: "IDKhuyenMaiChiTiet");

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_KhuyenMai_IDKhuyenMai",
                table: "KhuyenMaiChiTiet",
                column: "IDKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "IDKhuyenMai",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_KhuyenMaiChiTiet_sanPhamChiTiets_IDSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                column: "IDSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IDChiTietSp",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_KhuyenMai_IDKhuyenMai",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropForeignKey(
                name: "FK_KhuyenMaiChiTiet_sanPhamChiTiets_IDSanPhamChiTiet",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhuyenMaiChiTiet",
                table: "KhuyenMaiChiTiet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KhuyenMai",
                table: "KhuyenMai");

            migrationBuilder.RenameTable(
                name: "KhuyenMaiChiTiet",
                newName: "khuyenMaiChiTiets");

            migrationBuilder.RenameTable(
                name: "KhuyenMai",
                newName: "khuyenMais");

            migrationBuilder.RenameColumn(
                name: "TenMauSac",
                table: "mauSacs",
                newName: "Ten");

            migrationBuilder.RenameColumn(
                name: "MaMauSac",
                table: "mauSacs",
                newName: "Ma");

            migrationBuilder.RenameIndex(
                name: "IX_KhuyenMaiChiTiet_IDSanPhamChiTiet",
                table: "khuyenMaiChiTiets",
                newName: "IX_khuyenMaiChiTiets_IDSanPhamChiTiet");

            migrationBuilder.RenameIndex(
                name: "IX_KhuyenMaiChiTiet_IDKhuyenMai",
                table: "khuyenMaiChiTiets",
                newName: "IX_khuyenMaiChiTiets_IDKhuyenMai");

            migrationBuilder.AddPrimaryKey(
                name: "PK_khuyenMaiChiTiets",
                table: "khuyenMaiChiTiets",
                column: "IDKhuyenMaiChiTiet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_khuyenMais",
                table: "khuyenMais",
                column: "IDKhuyenMai");

            migrationBuilder.AddForeignKey(
                name: "FK_khuyenMaiChiTiets_khuyenMais_IDKhuyenMai",
                table: "khuyenMaiChiTiets",
                column: "IDKhuyenMai",
                principalTable: "khuyenMais",
                principalColumn: "IDKhuyenMai",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_khuyenMaiChiTiets_sanPhamChiTiets_IDSanPhamChiTiet",
                table: "khuyenMaiChiTiets",
                column: "IDSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IDChiTietSp",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

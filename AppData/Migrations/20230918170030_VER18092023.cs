using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class VER18092023 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChucVuNguoiDung");

            migrationBuilder.RenameColumn(
                name: "IdUser",
                table: "HoaDons",
                newName: "IdKhachHang");

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "IdChucVu",
                table: "NguoiDungs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "IdNguoiDung",
                table: "HoaDons",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    IdKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKhachHang = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenKhachHang = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.IdKhachHang);
                    table.ForeignKey(
                        name: "FK_KhachHang_NguoiDungs_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "IdNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamYeuThiches_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_IdChucVu",
                table: "NguoiDungs",
                column: "IdChucVu");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IdNguoiDung",
                table: "HoaDons",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_IdNguoiDung",
                table: "KhachHang",
                column: "IdNguoiDung");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDons_KhachHang_IdNguoiDung",
                table: "HoaDons",
                column: "IdNguoiDung",
                principalTable: "KhachHang",
                principalColumn: "IdKhachHang");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungs_chucVus_IdChucVu",
                table: "NguoiDungs",
                column: "IdChucVu",
                principalTable: "chucVus",
                principalColumn: "IdChucVu",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sanPhamYeuThiches_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                column: "IdSanPhamChiTiet",
                principalTable: "sanPhamChiTiets",
                principalColumn: "IdChiTietSp",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDons_KhachHang_IdNguoiDung",
                table: "HoaDons");

            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungs_chucVus_IdChucVu",
                table: "NguoiDungs");

            migrationBuilder.DropForeignKey(
                name: "FK_sanPhamYeuThiches_sanPhamChiTiets_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropIndex(
                name: "IX_sanPhamYeuThiches_IdSanPhamChiTiet",
                table: "sanPhamYeuThiches");

            migrationBuilder.DropIndex(
                name: "IX_NguoiDungs_IdChucVu",
                table: "NguoiDungs");

            migrationBuilder.DropIndex(
                name: "IX_HoaDons_IdNguoiDung",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "IdNguoiDung",
                table: "HoaDons");

            migrationBuilder.RenameColumn(
                name: "IdKhachHang",
                table: "HoaDons",
                newName: "IdUser");

            migrationBuilder.AlterColumn<string>(
                name: "IdSanPhamChiTiet",
                table: "sanPhamYeuThiches",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IdChucVu",
                table: "NguoiDungs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "ChucVuNguoiDung",
                columns: table => new
                {
                    ChucVuIdChucVu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NguoiDungsIdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVuNguoiDung", x => new { x.ChucVuIdChucVu, x.NguoiDungsIdNguoiDung });
                    table.ForeignKey(
                        name: "FK_ChucVuNguoiDung_chucVus_ChucVuIdChucVu",
                        column: x => x.ChucVuIdChucVu,
                        principalTable: "chucVus",
                        principalColumn: "IdChucVu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChucVuNguoiDung_NguoiDungs_NguoiDungsIdNguoiDung",
                        column: x => x.NguoiDungsIdNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "IdNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChucVuNguoiDung_NguoiDungsIdNguoiDung",
                table: "ChucVuNguoiDung",
                column: "NguoiDungsIdNguoiDung");
        }
    }
}

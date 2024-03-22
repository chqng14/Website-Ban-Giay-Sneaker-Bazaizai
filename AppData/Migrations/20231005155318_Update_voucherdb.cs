using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class Update_voucherdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhamViSanPham",
                table: "Vouchers");

            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LoaiHinhUuDai",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DieuKien",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "SoLuong",
                table: "Vouchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LoaiHinhUuDai",
                table: "Vouchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DieuKien",
                table: "Vouchers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "PhamViSanPham",
                table: "Vouchers",
                type: "nvarchar(300)",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class _05_10_2023_lan2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "GiaBan",
                table: "GioHangChiTiets",
                type: "float",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiaBan",
                table: "GioHangChiTiets");
        }
    }
}

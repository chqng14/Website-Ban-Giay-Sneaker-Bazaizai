using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class test2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NguoiDungs_voucherNguoiDungs_VoucherNguoiDungIdVouCherNguoiDung",
                table: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "VoucherVoucherNguoiDung");

            migrationBuilder.DropIndex(
                name: "IX_NguoiDungs_VoucherNguoiDungIdVouCherNguoiDung",
                table: "NguoiDungs");

            migrationBuilder.DropColumn(
                name: "VoucherNguoiDungIdVouCherNguoiDung",
                table: "NguoiDungs");

            migrationBuilder.CreateIndex(
                name: "IX_voucherNguoiDungs_IdNguoiDung",
                table: "voucherNguoiDungs",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_voucherNguoiDungs_IdVouCher",
                table: "voucherNguoiDungs",
                column: "IdVouCher");

            migrationBuilder.AddForeignKey(
                name: "FK_voucherNguoiDungs_NguoiDungs_IdNguoiDung",
                table: "voucherNguoiDungs",
                column: "IdNguoiDung",
                principalTable: "NguoiDungs",
                principalColumn: "IdNguoiDung",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_voucherNguoiDungs_vouchers_IdVouCher",
                table: "voucherNguoiDungs",
                column: "IdVouCher",
                principalTable: "vouchers",
                principalColumn: "IdVoucher",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_voucherNguoiDungs_NguoiDungs_IdNguoiDung",
                table: "voucherNguoiDungs");

            migrationBuilder.DropForeignKey(
                name: "FK_voucherNguoiDungs_vouchers_IdVouCher",
                table: "voucherNguoiDungs");

            migrationBuilder.DropIndex(
                name: "IX_voucherNguoiDungs_IdNguoiDung",
                table: "voucherNguoiDungs");

            migrationBuilder.DropIndex(
                name: "IX_voucherNguoiDungs_IdVouCher",
                table: "voucherNguoiDungs");

            migrationBuilder.AddColumn<Guid>(
                name: "VoucherNguoiDungIdVouCherNguoiDung",
                table: "NguoiDungs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "VoucherVoucherNguoiDung",
                columns: table => new
                {
                    VoucherNguoiDungsIdVouCherNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VouchersIdVoucher = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherVoucherNguoiDung", x => new { x.VoucherNguoiDungsIdVouCherNguoiDung, x.VouchersIdVoucher });
                    table.ForeignKey(
                        name: "FK_VoucherVoucherNguoiDung_voucherNguoiDungs_VoucherNguoiDungsIdVouCherNguoiDung",
                        column: x => x.VoucherNguoiDungsIdVouCherNguoiDung,
                        principalTable: "voucherNguoiDungs",
                        principalColumn: "IdVouCherNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoucherVoucherNguoiDung_vouchers_VouchersIdVoucher",
                        column: x => x.VouchersIdVoucher,
                        principalTable: "vouchers",
                        principalColumn: "IdVoucher",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_VoucherNguoiDungIdVouCherNguoiDung",
                table: "NguoiDungs",
                column: "VoucherNguoiDungIdVouCherNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherVoucherNguoiDung_VouchersIdVoucher",
                table: "VoucherVoucherNguoiDung",
                column: "VouchersIdVoucher");

            migrationBuilder.AddForeignKey(
                name: "FK_NguoiDungs_voucherNguoiDungs_VoucherNguoiDungIdVouCherNguoiDung",
                table: "NguoiDungs",
                column: "VoucherNguoiDungIdVouCherNguoiDung",
                principalTable: "voucherNguoiDungs",
                principalColumn: "IdVouCherNguoiDung");
        }
    }
}

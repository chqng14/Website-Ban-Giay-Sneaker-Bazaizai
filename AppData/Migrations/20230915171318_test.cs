﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatLieus",
                columns: table => new
                {
                    IDChatLieu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaChatLieu = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TenChatLieu = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLieus", x => x.IDChatLieu);
                });

            migrationBuilder.CreateTable(
                name: "chucVus",
                columns: table => new
                {
                    IdChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaChucVu = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenChucVu = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chucVus", x => x.IdChucVu);
                });

            migrationBuilder.CreateTable(
                name: "khuyenMais",
                columns: table => new
                {
                    IDKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoaiHinhKM = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    MucGiam = table.Column<decimal>(type: "decimal(18,0)", nullable: false, defaultValueSql: "((0))"),
                    PhamVi = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khuyenMais", x => x.IDKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "kichCos",
                columns: table => new
                {
                    IDKichCo = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaKichCo = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    SoKichCo = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kichCos", x => x.IDKichCo);
                });

            migrationBuilder.CreateTable(
                name: "kieuDeGiays",
                columns: table => new
                {
                    IdKieuDeGiay = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaKieuDeGiay = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TenKieuDeGiay = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    Trangthai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_kieuDeGiays", x => x.IdKieuDeGiay);
                });

            migrationBuilder.CreateTable(
                name: "LoaiGiays",
                columns: table => new
                {
                    IdLoaiGiay = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaLoaiGiay = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TenLoaiGiay = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiGiays", x => x.IdLoaiGiay);
                });

            migrationBuilder.CreateTable(
                name: "mauSacs",
                columns: table => new
                {
                    IDMauSac = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mauSacs", x => x.IDMauSac);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToans",
                columns: table => new
                {
                    IDPhuongThucThanhToan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaPhuongThucThanhToan = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TenPhuongThucThanhToan = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToans", x => x.IDPhuongThucThanhToan);
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    IdSanPham = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TenSanPham = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    Trangthai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.IdSanPham);
                });

            migrationBuilder.CreateTable(
                name: "thongTinGiaoHangs",
                columns: table => new
                {
                    IdThongTinGH = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thongTinGiaoHangs", x => x.IdThongTinGH);
                });

            migrationBuilder.CreateTable(
                name: "thuongHieus",
                columns: table => new
                {
                    IDThuongHieu = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MaThuongHieu = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thuongHieus", x => x.IDThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "voucherNguoiDungs",
                columns: table => new
                {
                    IdVouCherNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdVouCher = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_voucherNguoiDungs", x => x.IdVouCherNguoiDung);
                });

            migrationBuilder.CreateTable(
                name: "vouchers",
                columns: table => new
                {
                    IdVoucher = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaVoucher = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenVoucher = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    DieuKien = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    LoaiHinhUuDai = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    MucUuDai = table.Column<double>(type: "float", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vouchers", x => x.IdVoucher);
                });

            migrationBuilder.CreateTable(
                name: "xuatXus",
                columns: table => new
                {
                    IDXuatXu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ma = table.Column<string>(type: "varchar(50)", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_xuatXus", x => x.IDXuatXu);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDungs",
                columns: table => new
                {
                    IdNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    SDT = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    GioiTinh = table.Column<int>(type: "int", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    TenDangNhap = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    MatKhau = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    VoucherNguoiDungIdVouCherNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDungs", x => x.IdNguoiDung);
                    table.ForeignKey(
                        name: "FK_NguoiDungs_voucherNguoiDungs_VoucherNguoiDungIdVouCherNguoiDung",
                        column: x => x.VoucherNguoiDungIdVouCherNguoiDung,
                        principalTable: "voucherNguoiDungs",
                        principalColumn: "IdVouCherNguoiDung");
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdVoucher = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdThongTinGH = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MaHoaDon = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "DateTime", nullable: true),
                    NgayThanhToan = table.Column<DateTime>(type: "DateTime", nullable: true),
                    NgayShip = table.Column<DateTime>(type: "DateTime", nullable: true),
                    NgayNhan = table.Column<DateTime>(type: "DateTime", nullable: true),
                    TienShip = table.Column<double>(type: "float", nullable: true),
                    TienGiam = table.Column<double>(type: "float", nullable: true),
                    TongTien = table.Column<double>(type: "float", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    TrangThaiThanhToan = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDons", x => x.IdHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDons_thongTinGiaoHangs_IdThongTinGH",
                        column: x => x.IdThongTinGH,
                        principalTable: "thongTinGiaoHangs",
                        principalColumn: "IdThongTinGH");
                    table.ForeignKey(
                        name: "FK_HoaDons_vouchers_IdVoucher",
                        column: x => x.IdVoucher,
                        principalTable: "vouchers",
                        principalColumn: "IdVoucher");
                });

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

            migrationBuilder.CreateTable(
                name: "sanPhamChiTiets",
                columns: table => new
                {
                    IDChiTietSp = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ma = table.Column<string>(type: "varchar(50)", nullable: true),
                    Day = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    MoTa = table.Column<string>(type: "Nvarchar(max)", nullable: true),
                    SoLuongTon = table.Column<int>(type: "int", nullable: true),
                    GiaBan = table.Column<double>(type: "float", nullable: true),
                    GiaNhap = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    TrangThaiSale = table.Column<int>(type: "int", nullable: true),
                    IdSanPham = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKieuDeGiay = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdXuatXu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdChatLieu = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdMauSac = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdKichCo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdLoaiGiay = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdThuongHieu = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sanPhamChiTiets", x => x.IDChiTietSp);
                    table.ForeignKey(
                        name: "FK_sanPhamChiTiets_ChatLieus_IdChatLieu",
                        column: x => x.IdChatLieu,
                        principalTable: "ChatLieus",
                        principalColumn: "IDChatLieu");
                    table.ForeignKey(
                        name: "FK_sanPhamChiTiets_kichCos_IdKichCo",
                        column: x => x.IdKichCo,
                        principalTable: "kichCos",
                        principalColumn: "IDKichCo");
                    table.ForeignKey(
                        name: "FK_sanPhamChiTiets_kieuDeGiays_IdKieuDeGiay",
                        column: x => x.IdKieuDeGiay,
                        principalTable: "kieuDeGiays",
                        principalColumn: "IdKieuDeGiay");
                    table.ForeignKey(
                        name: "FK_sanPhamChiTiets_LoaiGiays_IdLoaiGiay",
                        column: x => x.IdLoaiGiay,
                        principalTable: "LoaiGiays",
                        principalColumn: "IdLoaiGiay");
                    table.ForeignKey(
                        name: "FK_sanPhamChiTiets_mauSacs_IdMauSac",
                        column: x => x.IdMauSac,
                        principalTable: "mauSacs",
                        principalColumn: "IDMauSac");
                    table.ForeignKey(
                        name: "FK_sanPhamChiTiets_SanPhams_IdSanPham",
                        column: x => x.IdSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "IdSanPham");
                    table.ForeignKey(
                        name: "FK_sanPhamChiTiets_thuongHieus_IdThuongHieu",
                        column: x => x.IdThuongHieu,
                        principalTable: "thuongHieus",
                        principalColumn: "IDThuongHieu");
                    table.ForeignKey(
                        name: "FK_sanPhamChiTiets_xuatXus_IdXuatXu",
                        column: x => x.IdXuatXu,
                        principalTable: "xuatXus",
                        principalColumn: "IDXuatXu");
                });

            migrationBuilder.CreateTable(
                name: "ChucVuNguoiDung",
                columns: table => new
                {
                    ChucVuIdChucVu = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NguoiDungsIdNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "gioHangs",
                columns: table => new
                {
                    IdNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))"),
                    NguoiDungIdNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gioHangs", x => x.IdNguoiDung);
                    table.ForeignKey(
                        name: "FK_gioHangs_NguoiDungs_NguoiDungIdNguoiDung",
                        column: x => x.NguoiDungIdNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "IdNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "sanPhamYeuThiches",
                columns: table => new
                {
                    IDSanPhamYeuThich = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IDNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDSanPhamChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sanPhamYeuThiches", x => x.IDSanPhamYeuThich);
                    table.ForeignKey(
                        name: "FK_sanPhamYeuThiches_NguoiDungs_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "NguoiDungs",
                        principalColumn: "IdNguoiDung",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phuongThucThanhToanChiTiets",
                columns: table => new
                {
                    IDPhuongThucThanhToanChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdThanhToan = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoTien = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phuongThucThanhToanChiTiets", x => x.IDPhuongThucThanhToanChiTiet);
                    table.ForeignKey(
                        name: "FK_phuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "IdHoaDon",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_phuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                        column: x => x.IdThanhToan,
                        principalTable: "PhuongThucThanhToans",
                        principalColumn: "IDPhuongThucThanhToan",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Anh",
                columns: table => new
                {
                    IDAnh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IdSanPhamChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anh", x => x.IDAnh);
                    table.ForeignKey(
                        name: "FK_Anh_sanPhamChiTiets_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "sanPhamChiTiets",
                        principalColumn: "IDChiTietSp",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hoaDonChiTiets",
                columns: table => new
                {
                    IdHoaDonChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdSanPhamChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    GiaGoc = table.Column<double>(type: "float", nullable: true),
                    GiaBan = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hoaDonChiTiets", x => x.IdHoaDonChiTiet);
                    table.ForeignKey(
                        name: "FK_hoaDonChiTiets_HoaDons_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "IdHoaDon");
                    table.ForeignKey(
                        name: "FK_hoaDonChiTiets_sanPhamChiTiets_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "sanPhamChiTiets",
                        principalColumn: "IDChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "khuyenMaiChiTiets",
                columns: table => new
                {
                    IDKhuyenMaiChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    IDKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDSanPhamChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_khuyenMaiChiTiets", x => x.IDKhuyenMaiChiTiet);
                    table.ForeignKey(
                        name: "FK_khuyenMaiChiTiets_khuyenMais_IDKhuyenMai",
                        column: x => x.IDKhuyenMai,
                        principalTable: "khuyenMais",
                        principalColumn: "IDKhuyenMai",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_khuyenMaiChiTiets_sanPhamChiTiets_IDSanPhamChiTiet",
                        column: x => x.IDSanPhamChiTiet,
                        principalTable: "sanPhamChiTiets",
                        principalColumn: "IDChiTietSp",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "gioHangChiTiets",
                columns: table => new
                {
                    IdGioHangChiTiet = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDNguoiDung = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IDSanPhamCT = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Soluong = table.Column<int>(type: "int", nullable: true),
                    GiaGoc = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gioHangChiTiets", x => x.IdGioHangChiTiet);
                    table.ForeignKey(
                        name: "FK_gioHangChiTiets_gioHangs_IDNguoiDung",
                        column: x => x.IDNguoiDung,
                        principalTable: "gioHangs",
                        principalColumn: "IdNguoiDung");
                    table.ForeignKey(
                        name: "FK_gioHangChiTiets_sanPhamChiTiets_IDSanPhamCT",
                        column: x => x.IDSanPhamCT,
                        principalTable: "sanPhamChiTiets",
                        principalColumn: "IDChiTietSp");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anh_IdSanPhamChiTiet",
                table: "Anh",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_ChucVuNguoiDung_NguoiDungsIdNguoiDung",
                table: "ChucVuNguoiDung",
                column: "NguoiDungsIdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_gioHangChiTiets_IDNguoiDung",
                table: "gioHangChiTiets",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_gioHangChiTiets_IDSanPhamCT",
                table: "gioHangChiTiets",
                column: "IDSanPhamCT");

            migrationBuilder.CreateIndex(
                name: "IX_gioHangs_NguoiDungIdNguoiDung",
                table: "gioHangs",
                column: "NguoiDungIdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_hoaDonChiTiets_IdHoaDon",
                table: "hoaDonChiTiets",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_hoaDonChiTiets_IdSanPhamChiTiet",
                table: "hoaDonChiTiets",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IdThongTinGH",
                table: "HoaDons",
                column: "IdThongTinGH");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IdVoucher",
                table: "HoaDons",
                column: "IdVoucher");

            migrationBuilder.CreateIndex(
                name: "IX_khuyenMaiChiTiets_IDKhuyenMai",
                table: "khuyenMaiChiTiets",
                column: "IDKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_khuyenMaiChiTiets_IDSanPhamChiTiet",
                table: "khuyenMaiChiTiets",
                column: "IDSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungs_VoucherNguoiDungIdVouCherNguoiDung",
                table: "NguoiDungs",
                column: "VoucherNguoiDungIdVouCherNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_NguoiDungThongTinGiaoHang_ThongTinGiaoHangsIdThongTinGH",
                table: "NguoiDungThongTinGiaoHang",
                column: "ThongTinGiaoHangsIdThongTinGH");

            migrationBuilder.CreateIndex(
                name: "IX_phuongThucThanhToanChiTiets_IdHoaDon",
                table: "phuongThucThanhToanChiTiets",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_phuongThucThanhToanChiTiets_IdThanhToan",
                table: "phuongThucThanhToanChiTiets",
                column: "IdThanhToan");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamChiTiets_IdChatLieu",
                table: "sanPhamChiTiets",
                column: "IdChatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamChiTiets_IdKichCo",
                table: "sanPhamChiTiets",
                column: "IdKichCo");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamChiTiets_IdKieuDeGiay",
                table: "sanPhamChiTiets",
                column: "IdKieuDeGiay");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamChiTiets_IdLoaiGiay",
                table: "sanPhamChiTiets",
                column: "IdLoaiGiay");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamChiTiets_IdMauSac",
                table: "sanPhamChiTiets",
                column: "IdMauSac");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamChiTiets_IdSanPham",
                table: "sanPhamChiTiets",
                column: "IdSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamChiTiets_IdThuongHieu",
                table: "sanPhamChiTiets",
                column: "IdThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamChiTiets_IdXuatXu",
                table: "sanPhamChiTiets",
                column: "IdXuatXu");

            migrationBuilder.CreateIndex(
                name: "IX_sanPhamYeuThiches_IDNguoiDung",
                table: "sanPhamYeuThiches",
                column: "IDNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherVoucherNguoiDung_VouchersIdVoucher",
                table: "VoucherVoucherNguoiDung",
                column: "VouchersIdVoucher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anh");

            migrationBuilder.DropTable(
                name: "ChucVuNguoiDung");

            migrationBuilder.DropTable(
                name: "gioHangChiTiets");

            migrationBuilder.DropTable(
                name: "hoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "khuyenMaiChiTiets");

            migrationBuilder.DropTable(
                name: "NguoiDungThongTinGiaoHang");

            migrationBuilder.DropTable(
                name: "phuongThucThanhToanChiTiets");

            migrationBuilder.DropTable(
                name: "sanPhamYeuThiches");

            migrationBuilder.DropTable(
                name: "VoucherVoucherNguoiDung");

            migrationBuilder.DropTable(
                name: "chucVus");

            migrationBuilder.DropTable(
                name: "gioHangs");

            migrationBuilder.DropTable(
                name: "khuyenMais");

            migrationBuilder.DropTable(
                name: "sanPhamChiTiets");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToans");

            migrationBuilder.DropTable(
                name: "NguoiDungs");

            migrationBuilder.DropTable(
                name: "ChatLieus");

            migrationBuilder.DropTable(
                name: "kichCos");

            migrationBuilder.DropTable(
                name: "kieuDeGiays");

            migrationBuilder.DropTable(
                name: "LoaiGiays");

            migrationBuilder.DropTable(
                name: "mauSacs");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "thuongHieus");

            migrationBuilder.DropTable(
                name: "xuatXus");

            migrationBuilder.DropTable(
                name: "thongTinGiaoHangs");

            migrationBuilder.DropTable(
                name: "vouchers");

            migrationBuilder.DropTable(
                name: "voucherNguoiDungs");
        }
    }
}

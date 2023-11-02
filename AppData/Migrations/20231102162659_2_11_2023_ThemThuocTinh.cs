using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class _2_11_2023_ThemThuocTinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatLieu",
                columns: table => new
                {
                    IdChatLieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaChatLieu = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenChatLieu = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLieu", x => x.IdChatLieu);
                });

            migrationBuilder.CreateTable(
                name: "ChucVu",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaChucVu = table.Column<string>(type: "varchar(50)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChucVu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    IdKhuyenMai = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKhuyenMai = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime", nullable: false),
                    LoaiHinhKM = table.Column<int>(type: "int", maxLength: 1000, nullable: false),
                    MucGiam = table.Column<decimal>(type: "decimal(18,0)", nullable: false, defaultValueSql: "((0))"),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.IdKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "KichCo",
                columns: table => new
                {
                    IdKichCo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKichCo = table.Column<string>(type: "varchar(50)", nullable: true),
                    SoKichCo = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KichCo", x => x.IdKichCo);
                });

            migrationBuilder.CreateTable(
                name: "KieuDeGiay",
                columns: table => new
                {
                    IdKieuDeGiay = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKieuDeGiay = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenKieuDeGiay = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Trangthai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KieuDeGiay", x => x.IdKieuDeGiay);
                });

            migrationBuilder.CreateTable(
                name: "LoaiGiay",
                columns: table => new
                {
                    IdLoaiGiay = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLoaiGiay = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenLoaiGiay = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiGiay", x => x.IdLoaiGiay);
                });

            migrationBuilder.CreateTable(
                name: "MauSac",
                columns: table => new
                {
                    IdMauSac = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaMauSac = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenMauSac = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauSac", x => x.IdMauSac);
                });

            migrationBuilder.CreateTable(
                name: "NguoiDung",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "(newid())"),
                    MaNguoiDung = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(256)", nullable: true),
                    GioiTinh = table.Column<int>(type: "int", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgaySinh = table.Column<DateTime>(type: "datetime", nullable: true),
                    TongChiTieu = table.Column<double>(type: "float", nullable: true),
                    SuaDoi = table.Column<int>(type: "int", nullable: true),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NguoiDung", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToan",
                columns: table => new
                {
                    IdPhuongThucThanhToan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaPhuongThucThanhToan = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenPhuongThucThanhToan = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToan", x => x.IdPhuongThucThanhToan);
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    IdSanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSanPham = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenSanPham = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Trangthai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPham", x => x.IdSanPham);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    IdThuongHieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaThuongHieu = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieu", x => x.IdThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    IdVoucher = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaVoucher = table.Column<string>(type: "varchar(50)", nullable: true),
                    TenVoucher = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DieuKien = table.Column<int>(type: "int", nullable: false),
                    LoaiHinhUuDai = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    MucUuDai = table.Column<double>(type: "float", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.IdVoucher);
                });

            migrationBuilder.CreateTable(
                name: "XuatXu",
                columns: table => new
                {
                    IdXuatXu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ma = table.Column<string>(type: "varchar(50)", nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XuatXu", x => x.IdXuatXu);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_ChucVu_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ChucVu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_ChucVu_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ChucVu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_NguoiDung_UserId",
                        column: x => x.UserId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHang",
                columns: table => new
                {
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    NguoiDungId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHang", x => x.IdNguoiDung);
                    table.ForeignKey(
                        name: "FK_GioHang_NguoiDung_NguoiDungId",
                        column: x => x.NguoiDungId,
                        principalTable: "NguoiDung",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KhachHang",
                columns: table => new
                {
                    IdKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenKhachHang = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhachHang", x => x.IdKhachHang);
                    table.ForeignKey(
                        name: "FK_KhachHang_NguoiDung_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThongTinGiaoHang",
                columns: table => new
                {
                    IdThongTinGH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinGiaoHang", x => x.IdThongTinGH);
                    table.ForeignKey(
                        name: "FK_ThongTinGiaoHang_NguoiDung_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoucherNguoiDung",
                columns: table => new
                {
                    IdVouCherNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdVouCher = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherNguoiDung", x => x.IdVouCherNguoiDung);
                    table.ForeignKey(
                        name: "FK_VoucherNguoiDung_NguoiDung_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoucherNguoiDung_Voucher_IdVouCher",
                        column: x => x.IdVouCher,
                        principalTable: "Voucher",
                        principalColumn: "IdVoucher");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamChiTiet",
                columns: table => new
                {
                    IdChiTietSp = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ma = table.Column<string>(type: "varchar(50)", nullable: true),
                    Day = table.Column<bool>(type: "bit", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuongTon = table.Column<int>(type: "int", nullable: true),
                    SoLuongDaBan = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiBat = table.Column<bool>(type: "bit", nullable: true),
                    GiaBan = table.Column<double>(type: "float", nullable: true),
                    GiaNhap = table.Column<double>(type: "float", nullable: true),
                    GiaThucTe = table.Column<double>(type: "float", nullable: true),
                    KhoiLuong = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    TrangThaiSale = table.Column<int>(type: "int", nullable: true),
                    IdSanPham = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdKieuDeGiay = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdXuatXu = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdChatLieu = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdMauSac = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdKichCo = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdLoaiGiay = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdThuongHieu = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamChiTiet", x => x.IdChiTietSp);
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiet_ChatLieu_IdChatLieu",
                        column: x => x.IdChatLieu,
                        principalTable: "ChatLieu",
                        principalColumn: "IdChatLieu");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiet_KichCo_IdKichCo",
                        column: x => x.IdKichCo,
                        principalTable: "KichCo",
                        principalColumn: "IdKichCo");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiet_KieuDeGiay_IdKieuDeGiay",
                        column: x => x.IdKieuDeGiay,
                        principalTable: "KieuDeGiay",
                        principalColumn: "IdKieuDeGiay");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiet_LoaiGiay_IdLoaiGiay",
                        column: x => x.IdLoaiGiay,
                        principalTable: "LoaiGiay",
                        principalColumn: "IdLoaiGiay");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiet_MauSac_IdMauSac",
                        column: x => x.IdMauSac,
                        principalTable: "MauSac",
                        principalColumn: "IdMauSac");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiet_SanPham_IdSanPham",
                        column: x => x.IdSanPham,
                        principalTable: "SanPham",
                        principalColumn: "IdSanPham");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiet_ThuongHieu_IdThuongHieu",
                        column: x => x.IdThuongHieu,
                        principalTable: "ThuongHieu",
                        principalColumn: "IdThuongHieu");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiet_XuatXu_IdXuatXu",
                        column: x => x.IdXuatXu,
                        principalTable: "XuatXu",
                        principalColumn: "IdXuatXu");
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    IdHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdNguoiSuaGanNhat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdKhachHang = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdVoucher = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdThongTinGH = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MaHoaDon = table.Column<string>(type: "varchar(50)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "DateTime", nullable: true),
                    NgayThanhToan = table.Column<DateTime>(type: "DateTime", nullable: true),
                    NgayShip = table.Column<DateTime>(type: "DateTime", nullable: true),
                    NgayNhan = table.Column<DateTime>(type: "DateTime", nullable: true),
                    NgayGiaoDuKien = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TienShip = table.Column<double>(type: "float", nullable: true),
                    TienGiam = table.Column<double>(type: "float", nullable: true),
                    TongTien = table.Column<double>(type: "float", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(MAX)", nullable: true),
                    TrangThaiGiaoHang = table.Column<int>(type: "int", nullable: true),
                    TrangThaiThanhToan = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDon", x => x.IdHoaDon);
                    table.ForeignKey(
                        name: "FK_HoaDon_KhachHang_IdKhachHang",
                        column: x => x.IdKhachHang,
                        principalTable: "KhachHang",
                        principalColumn: "IdKhachHang");
                    table.ForeignKey(
                        name: "FK_HoaDon_NguoiDung_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HoaDon_ThongTinGiaoHang_IdThongTinGH",
                        column: x => x.IdThongTinGH,
                        principalTable: "ThongTinGiaoHang",
                        principalColumn: "IdThongTinGH");
                    table.ForeignKey(
                        name: "FK_HoaDon_Voucher_IdVoucher",
                        column: x => x.IdVoucher,
                        principalTable: "Voucher",
                        principalColumn: "IdVoucher");
                });

            migrationBuilder.CreateTable(
                name: "Anh",
                columns: table => new
                {
                    IdAnh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anh", x => x.IdAnh);
                    table.ForeignKey(
                        name: "FK_Anh_SanPhamChiTiet_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiet",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "DanhGia",
                columns: table => new
                {
                    IdDanhGia = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BinhLuan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayDanhGia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SaoSp = table.Column<int>(type: "int", nullable: true),
                    SaoVanChuyen = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false, defaultValueSql: "((0))"),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChatLuongSanPham = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SuaDoi = table.Column<int>(type: "int", nullable: false),
                    LuotYeuThich = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGia", x => x.IdDanhGia);
                    table.ForeignKey(
                        name: "FK_DanhGia_DanhGia_ParentId",
                        column: x => x.ParentId,
                        principalTable: "DanhGia",
                        principalColumn: "IdDanhGia");
                    table.ForeignKey(
                        name: "FK_DanhGia_NguoiDung_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DanhGia_SanPhamChiTiet_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiet",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "GioHangChiTiet",
                columns: table => new
                {
                    IdGioHangChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamCT = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Soluong = table.Column<int>(type: "int", nullable: true),
                    GiaGoc = table.Column<double>(type: "float", nullable: true),
                    GiaBan = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangChiTiet", x => x.IdGioHangChiTiet);
                    table.ForeignKey(
                        name: "FK_GioHangChiTiet_GioHang_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "GioHang",
                        principalColumn: "IdNguoiDung");
                    table.ForeignKey(
                        name: "FK_GioHangChiTiet_SanPhamChiTiet_IdSanPhamCT",
                        column: x => x.IdSanPhamCT,
                        principalTable: "SanPhamChiTiet",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMaiChiTiet",
                columns: table => new
                {
                    IdKhuyenMaiChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdKhuyenMai = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMaiChiTiet", x => x.IdKhuyenMaiChiTiet);
                    table.ForeignKey(
                        name: "FK_KhuyenMaiChiTiet_KhuyenMai_IdKhuyenMai",
                        column: x => x.IdKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "IdKhuyenMai");
                    table.ForeignKey(
                        name: "FK_KhuyenMaiChiTiet_SanPhamChiTiet_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiet",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamYeuThich",
                columns: table => new
                {
                    IdSanPhamYeuThich = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamYeuThich", x => x.IdSanPhamYeuThich);
                    table.ForeignKey(
                        name: "FK_SanPhamYeuThich_NguoiDung_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "NguoiDung",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SanPhamYeuThich_SanPhamChiTiet_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiet",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "HoaDonChiTiet",
                columns: table => new
                {
                    IdHoaDonChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    GiaGoc = table.Column<double>(type: "float", nullable: true),
                    GiaBan = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoaDonChiTiet", x => x.IdHoaDonChiTiet);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "IdHoaDon");
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiet_SanPhamChiTiet_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiet",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToanChiTiet",
                columns: table => new
                {
                    IdPhuongThucThanhToanChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdThanhToan = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoTien = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToanChiTiet", x => x.IdPhuongThucThanhToanChiTiet);
                    table.ForeignKey(
                        name: "FK_PhuongThucThanhToanChiTiet_HoaDon_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "IdHoaDon");
                    table.ForeignKey(
                        name: "FK_PhuongThucThanhToanChiTiet_PhuongThucThanhToan_IdThanhToan",
                        column: x => x.IdThanhToan,
                        principalTable: "PhuongThucThanhToan",
                        principalColumn: "IdPhuongThucThanhToan");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anh_IdSanPhamChiTiet",
                table: "Anh",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "ChucVu",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_IdNguoiDung",
                table: "DanhGia",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_IdSanPhamChiTiet",
                table: "DanhGia",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_ParentId",
                table: "DanhGia",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHang_NguoiDungId",
                table: "GioHang",
                column: "NguoiDungId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiet_IdNguoiDung",
                table: "GioHangChiTiet",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiet_IdSanPhamCT",
                table: "GioHangChiTiet",
                column: "IdSanPhamCT");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IdKhachHang",
                table: "HoaDon",
                column: "IdKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IdNguoiDung",
                table: "HoaDon",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IdThongTinGH",
                table: "HoaDon",
                column: "IdThongTinGH");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_IdVoucher",
                table: "HoaDon",
                column: "IdVoucher");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_IdHoaDon",
                table: "HoaDonChiTiet",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiet_IdSanPhamChiTiet",
                table: "HoaDonChiTiet",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_KhachHang_IdNguoiDung",
                table: "KhachHang",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMaiChiTiet_IdKhuyenMai",
                table: "KhuyenMaiChiTiet",
                column: "IdKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_KhuyenMaiChiTiet_IdSanPhamChiTiet",
                table: "KhuyenMaiChiTiet",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "NguoiDung",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "NguoiDung",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PhuongThucThanhToanChiTiet_IdHoaDon",
                table: "PhuongThucThanhToanChiTiet",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_PhuongThucThanhToanChiTiet_IdThanhToan",
                table: "PhuongThucThanhToanChiTiet",
                column: "IdThanhToan");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiet_IdChatLieu",
                table: "SanPhamChiTiet",
                column: "IdChatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiet_IdKichCo",
                table: "SanPhamChiTiet",
                column: "IdKichCo");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiet_IdKieuDeGiay",
                table: "SanPhamChiTiet",
                column: "IdKieuDeGiay");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiet_IdLoaiGiay",
                table: "SanPhamChiTiet",
                column: "IdLoaiGiay");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiet_IdMauSac",
                table: "SanPhamChiTiet",
                column: "IdMauSac");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiet_IdSanPham",
                table: "SanPhamChiTiet",
                column: "IdSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiet_IdThuongHieu",
                table: "SanPhamChiTiet",
                column: "IdThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiet_IdXuatXu",
                table: "SanPhamChiTiet",
                column: "IdXuatXu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamYeuThich_IdNguoiDung",
                table: "SanPhamYeuThich",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamYeuThich_IdSanPhamChiTiet",
                table: "SanPhamYeuThich",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinGiaoHang_IdNguoiDung",
                table: "ThongTinGiaoHang",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherNguoiDung_IdNguoiDung",
                table: "VoucherNguoiDung",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherNguoiDung_IdVouCher",
                table: "VoucherNguoiDung",
                column: "IdVouCher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anh");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DanhGia");

            migrationBuilder.DropTable(
                name: "GioHangChiTiet");

            migrationBuilder.DropTable(
                name: "HoaDonChiTiet");

            migrationBuilder.DropTable(
                name: "KhuyenMaiChiTiet");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToanChiTiet");

            migrationBuilder.DropTable(
                name: "SanPhamYeuThich");

            migrationBuilder.DropTable(
                name: "VoucherNguoiDung");

            migrationBuilder.DropTable(
                name: "ChucVu");

            migrationBuilder.DropTable(
                name: "GioHang");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToan");

            migrationBuilder.DropTable(
                name: "SanPhamChiTiet");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "ThongTinGiaoHang");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.DropTable(
                name: "ChatLieu");

            migrationBuilder.DropTable(
                name: "KichCo");

            migrationBuilder.DropTable(
                name: "KieuDeGiay");

            migrationBuilder.DropTable(
                name: "LoaiGiay");

            migrationBuilder.DropTable(
                name: "MauSac");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "ThuongHieu");

            migrationBuilder.DropTable(
                name: "XuatXu");

            migrationBuilder.DropTable(
                name: "NguoiDung");
        }
    }
}

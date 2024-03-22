using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App_Data.Migrations
{
    public partial class capNhapLaiBangSPCT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatLieus",
                columns: table => new
                {
                    IdChatLieu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaChatLieu = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TenChatLieu = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLieus", x => x.IdChatLieu);
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMai",
                columns: table => new
                {
                    IdKhuyenMai = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "(newid())"),
                    MaKhuyenMai = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    TenKhuyenMai = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime", nullable: true),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime", nullable: true),
                    LoaiHinhKM = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    MucGiam = table.Column<decimal>(type: "decimal(18,0)", nullable: true, defaultValueSql: "((0))"),
                    PhamVi = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KhuyenMai", x => x.IdKhuyenMai);
                });

            migrationBuilder.CreateTable(
                name: "KichCos",
                columns: table => new
                {
                    IdKichCo = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "(newid())"),
                    MaKichCo = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    SoKichCo = table.Column<int>(type: "int", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KichCos", x => x.IdKichCo);
                });

            migrationBuilder.CreateTable(
                name: "KieuDeGiays",
                columns: table => new
                {
                    IdKieuDeGiay = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKieuDeGiay = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TenKieuDeGiay = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Trangthai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KieuDeGiays", x => x.IdKieuDeGiay);
                });

            migrationBuilder.CreateTable(
                name: "LoaiGiays",
                columns: table => new
                {
                    IdLoaiGiay = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaLoaiGiay = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TenLoaiGiay = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoaiGiays", x => x.IdLoaiGiay);
                });

            migrationBuilder.CreateTable(
                name: "MauSacs",
                columns: table => new
                {
                    IdMauSac = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaMauSac = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TenMauSac = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauSacs", x => x.IdMauSac);
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToans",
                columns: table => new
                {
                    IdPhuongThucThanhToan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaPhuongThucThanhToan = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TenPhuongThucThanhToan = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToans", x => x.IdPhuongThucThanhToan);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaChucVu = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SanPhams",
                columns: table => new
                {
                    IdSanPham = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaSanPham = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TenSanPham = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Trangthai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhams", x => x.IdSanPham);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieus",
                columns: table => new
                {
                    IdThuongHieu = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "(newid())"),
                    MaThuongHieu = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuongHieus", x => x.IdThuongHieu);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "(newid())"),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    GioiTinh = table.Column<int>(type: "int", nullable: true),
                    NgaySinh = table.Column<DateTime>(type: "datetime", nullable: true),
                    AnhDaiDien = table.Column<string>(type: "nvarchar(300)", nullable: true),
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
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    IdVoucher = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaVoucher = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    TenVoucher = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DieuKien = table.Column<int>(type: "int", nullable: true),
                    PhamViSanPham = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    LoaiHinhUuDai = table.Column<int>(type: "int", nullable: true),
                    SoLuong = table.Column<int>(type: "int", nullable: true),
                    MucUuDai = table.Column<double>(type: "float", nullable: true),
                    NgayBatDau = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NgayKetThuc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.IdVoucher);
                });

            migrationBuilder.CreateTable(
                name: "XuatXus",
                columns: table => new
                {
                    IdXuatXu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ma = table.Column<string>(type: "varchar(50)", nullable: true),
                    Ten = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_XuatXus", x => x.IdXuatXu);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
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
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHangs",
                columns: table => new
                {
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    NguoiDungId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangs", x => x.IdNguoiDung);
                    table.ForeignKey(
                        name: "FK_GioHangs_Users_NguoiDungId",
                        column: x => x.NguoiDungId,
                        principalTable: "Users",
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
                        name: "FK_KhachHang_Users_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ThongTinGiaoHangs",
                columns: table => new
                {
                    IdThongTinGH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TenNguoiNhan = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    SDT = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    DiaChi = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinGiaoHangs", x => x.IdThongTinGH);
                    table.ForeignKey(
                        name: "FK_ThongTinGiaoHangs_Users_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
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
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoucherNguoiDungs",
                columns: table => new
                {
                    IdVouCherNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdVouCher = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoucherNguoiDungs", x => x.IdVouCherNguoiDung);
                    table.ForeignKey(
                        name: "FK_VoucherNguoiDungs_Users_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoucherNguoiDungs_Vouchers_IdVouCher",
                        column: x => x.IdVouCher,
                        principalTable: "Vouchers",
                        principalColumn: "IdVoucher");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamChiTiets",
                columns: table => new
                {
                    IdChiTietSp = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Ma = table.Column<string>(type: "varchar(50)", nullable: true),
                    Day = table.Column<string>(type: "nvarchar(250)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoLuongTon = table.Column<int>(type: "int", nullable: true),
                    SoLuongDaBan = table.Column<int>(type: "int", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NoiBat = table.Column<bool>(type: "bit", nullable: true),
                    GiaBan = table.Column<double>(type: "float", nullable: true),
                    GiaNhap = table.Column<double>(type: "float", nullable: true),
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
                    table.PrimaryKey("PK_SanPhamChiTiets", x => x.IdChiTietSp);
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiets_ChatLieus_IdChatLieu",
                        column: x => x.IdChatLieu,
                        principalTable: "ChatLieus",
                        principalColumn: "IdChatLieu");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiets_KichCos_IdKichCo",
                        column: x => x.IdKichCo,
                        principalTable: "KichCos",
                        principalColumn: "IdKichCo");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiets_KieuDeGiays_IdKieuDeGiay",
                        column: x => x.IdKieuDeGiay,
                        principalTable: "KieuDeGiays",
                        principalColumn: "IdKieuDeGiay");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiets_LoaiGiays_IdLoaiGiay",
                        column: x => x.IdLoaiGiay,
                        principalTable: "LoaiGiays",
                        principalColumn: "IdLoaiGiay");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiets_MauSacs_IdMauSac",
                        column: x => x.IdMauSac,
                        principalTable: "MauSacs",
                        principalColumn: "IdMauSac");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiets_SanPhams_IdSanPham",
                        column: x => x.IdSanPham,
                        principalTable: "SanPhams",
                        principalColumn: "IdSanPham");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiets_ThuongHieus_IdThuongHieu",
                        column: x => x.IdThuongHieu,
                        principalTable: "ThuongHieus",
                        principalColumn: "IdThuongHieu");
                    table.ForeignKey(
                        name: "FK_SanPhamChiTiets_XuatXus_IdXuatXu",
                        column: x => x.IdXuatXu,
                        principalTable: "XuatXus",
                        principalColumn: "IdXuatXu");
                });

            migrationBuilder.CreateTable(
                name: "HoaDons",
                columns: table => new
                {
                    IdHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdKhachHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdVoucher = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdThongTinGH = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        name: "FK_HoaDons_KhachHang_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "KhachHang",
                        principalColumn: "IdKhachHang");
                    table.ForeignKey(
                        name: "FK_HoaDons_ThongTinGiaoHangs_IdThongTinGH",
                        column: x => x.IdThongTinGH,
                        principalTable: "ThongTinGiaoHangs",
                        principalColumn: "IdThongTinGH");
                    table.ForeignKey(
                        name: "FK_HoaDons_Vouchers_IdVoucher",
                        column: x => x.IdVoucher,
                        principalTable: "Vouchers",
                        principalColumn: "IdVoucher");
                });

            migrationBuilder.CreateTable(
                name: "Anh",
                columns: table => new
                {
                    IdAnh = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anh", x => x.IdAnh);
                    table.ForeignKey(
                        name: "FK_Anh_SanPhamChiTiets_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiets",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "GioHangChiTiets",
                columns: table => new
                {
                    IdGioHangChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamCT = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Soluong = table.Column<int>(type: "int", nullable: true),
                    GiaGoc = table.Column<double>(type: "float", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangChiTiets", x => x.IdGioHangChiTiet);
                    table.ForeignKey(
                        name: "FK_GioHangChiTiets_GioHangs_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "GioHangs",
                        principalColumn: "IdNguoiDung");
                    table.ForeignKey(
                        name: "FK_GioHangChiTiets_SanPhamChiTiets_IdSanPhamCT",
                        column: x => x.IdSanPhamCT,
                        principalTable: "SanPhamChiTiets",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "KhuyenMaiChiTiet",
                columns: table => new
                {
                    IdKhuyenMaiChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "(newid())"),
                    IdKhuyenMai = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                        name: "FK_KhuyenMaiChiTiet_SanPhamChiTiets_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiets",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamYeuThichs",
                columns: table => new
                {
                    IdSanPhamYeuThich = table.Column<string>(type: "nvarchar(450)", nullable: false, defaultValueSql: "(newid())"),
                    IdNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdSanPhamChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanPhamYeuThichs", x => x.IdSanPhamYeuThich);
                    table.ForeignKey(
                        name: "FK_SanPhamYeuThichs_SanPhamChiTiets_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiets",
                        principalColumn: "IdChiTietSp");
                    table.ForeignKey(
                        name: "FK_SanPhamYeuThichs_Users_IdNguoiDung",
                        column: x => x.IdNguoiDung,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HoaDonChiTiets",
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
                    table.PrimaryKey("PK_HoaDonChiTiets", x => x.IdHoaDonChiTiet);
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiets_HoaDons_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "IdHoaDon");
                    table.ForeignKey(
                        name: "FK_HoaDonChiTiets_SanPhamChiTiets_IdSanPhamChiTiet",
                        column: x => x.IdSanPhamChiTiet,
                        principalTable: "SanPhamChiTiets",
                        principalColumn: "IdChiTietSp");
                });

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToanChiTiets",
                columns: table => new
                {
                    IdPhuongThucThanhToanChiTiet = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdHoaDon = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdThanhToan = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SoTien = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToanChiTiets", x => x.IdPhuongThucThanhToanChiTiet);
                    table.ForeignKey(
                        name: "FK_PhuongThucThanhToanChiTiets_HoaDons_IdHoaDon",
                        column: x => x.IdHoaDon,
                        principalTable: "HoaDons",
                        principalColumn: "IdHoaDon");
                    table.ForeignKey(
                        name: "FK_PhuongThucThanhToanChiTiets_PhuongThucThanhToans_IdThanhToan",
                        column: x => x.IdThanhToan,
                        principalTable: "PhuongThucThanhToans",
                        principalColumn: "IdPhuongThucThanhToan");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anh_IdSanPhamChiTiet",
                table: "Anh",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiets_IdNguoiDung",
                table: "GioHangChiTiets",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangChiTiets_IdSanPhamCT",
                table: "GioHangChiTiets",
                column: "IdSanPhamCT");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangs_NguoiDungId",
                table: "GioHangs",
                column: "NguoiDungId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiets_IdHoaDon",
                table: "HoaDonChiTiets",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiets_IdSanPhamChiTiet",
                table: "HoaDonChiTiets",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IdNguoiDung",
                table: "HoaDons",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IdThongTinGH",
                table: "HoaDons",
                column: "IdThongTinGH");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDons_IdVoucher",
                table: "HoaDons",
                column: "IdVoucher");

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
                name: "IX_PhuongThucThanhToanChiTiets_IdHoaDon",
                table: "PhuongThucThanhToanChiTiets",
                column: "IdHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_PhuongThucThanhToanChiTiets_IdThanhToan",
                table: "PhuongThucThanhToanChiTiets",
                column: "IdThanhToan");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiets_IdChatLieu",
                table: "SanPhamChiTiets",
                column: "IdChatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiets_IdKichCo",
                table: "SanPhamChiTiets",
                column: "IdKichCo");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiets_IdKieuDeGiay",
                table: "SanPhamChiTiets",
                column: "IdKieuDeGiay");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiets_IdLoaiGiay",
                table: "SanPhamChiTiets",
                column: "IdLoaiGiay");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiets_IdMauSac",
                table: "SanPhamChiTiets",
                column: "IdMauSac");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiets_IdSanPham",
                table: "SanPhamChiTiets",
                column: "IdSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiets_IdThuongHieu",
                table: "SanPhamChiTiets",
                column: "IdThuongHieu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamChiTiets_IdXuatXu",
                table: "SanPhamChiTiets",
                column: "IdXuatXu");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamYeuThichs_IdNguoiDung",
                table: "SanPhamYeuThichs",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamYeuThichs_IdSanPhamChiTiet",
                table: "SanPhamYeuThichs",
                column: "IdSanPhamChiTiet");

            migrationBuilder.CreateIndex(
                name: "IX_ThongTinGiaoHangs_IdNguoiDung",
                table: "ThongTinGiaoHangs",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherNguoiDungs_IdNguoiDung",
                table: "VoucherNguoiDungs",
                column: "IdNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_VoucherNguoiDungs_IdVouCher",
                table: "VoucherNguoiDungs",
                column: "IdVouCher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Anh");

            migrationBuilder.DropTable(
                name: "GioHangChiTiets");

            migrationBuilder.DropTable(
                name: "HoaDonChiTiets");

            migrationBuilder.DropTable(
                name: "KhuyenMaiChiTiet");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToanChiTiets");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SanPhamYeuThichs");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "VoucherNguoiDungs");

            migrationBuilder.DropTable(
                name: "GioHangs");

            migrationBuilder.DropTable(
                name: "KhuyenMai");

            migrationBuilder.DropTable(
                name: "HoaDons");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToans");

            migrationBuilder.DropTable(
                name: "SanPhamChiTiets");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "KhachHang");

            migrationBuilder.DropTable(
                name: "ThongTinGiaoHangs");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropTable(
                name: "ChatLieus");

            migrationBuilder.DropTable(
                name: "KichCos");

            migrationBuilder.DropTable(
                name: "KieuDeGiays");

            migrationBuilder.DropTable(
                name: "LoaiGiays");

            migrationBuilder.DropTable(
                name: "MauSacs");

            migrationBuilder.DropTable(
                name: "SanPhams");

            migrationBuilder.DropTable(
                name: "ThuongHieus");

            migrationBuilder.DropTable(
                name: "XuatXus");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

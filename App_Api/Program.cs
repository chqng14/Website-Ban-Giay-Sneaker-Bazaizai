using App_Api.Controllers;
using App_Api.Helpers.CustomJson;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = new CustomJsonNamingPolicy();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<SanPhamChiTietController>();
builder.Services.AddScoped<IAllRepo<Anh>, AllRepo<Anh>>();
builder.Services.AddScoped<IAllRepo<ChatLieu>, AllRepo<ChatLieu>>();
builder.Services.AddScoped<IAllRepo<ChucVu>, AllRepo<ChucVu>>();
builder.Services.AddScoped<IAllRepo<GioHang>, AllRepo<GioHang>>();
builder.Services.AddScoped<IAllRepo<GioHangChiTiet>, AllRepo<GioHangChiTiet>>();
builder.Services.AddScoped<IAllRepo<HoaDon>, AllRepo<HoaDon>>();
builder.Services.AddScoped<IAllRepo<HoaDonChiTiet>, AllRepo<HoaDonChiTiet>>();
builder.Services.AddScoped<IAllRepo<KhuyenMai>, AllRepo<KhuyenMai>>();
builder.Services.AddScoped<IAllRepo<KhuyenMaiChiTiet>, AllRepo<KhuyenMaiChiTiet>>();
builder.Services.AddScoped<IAllRepo<KichCo>, AllRepo<KichCo>>();
builder.Services.AddScoped<IAllRepo<KieuDeGiay>, AllRepo<KieuDeGiay>>();
builder.Services.AddScoped<IAllRepo<LoaiGiay>, AllRepo<LoaiGiay>>();
builder.Services.AddScoped<IAllRepo<MauSac>, AllRepo<MauSac>>();
builder.Services.AddScoped<IAllRepo<NguoiDung>, AllRepo<NguoiDung>>();
builder.Services.AddScoped<IAllRepo<PhuongThucThanhToan>, AllRepo<PhuongThucThanhToan>>();
builder.Services.AddScoped<IAllRepo<PhuongThucThanhToanChiTiet>, AllRepo<PhuongThucThanhToanChiTiet>>();
builder.Services.AddScoped<IAllRepo<SanPham>, AllRepo<SanPham>>();
builder.Services.AddScoped<IAllRepo<SanPhamChiTiet>, AllRepo<SanPhamChiTiet>>();
builder.Services.AddScoped<IAllRepo<SanPhamYeuThich>, AllRepo<SanPhamYeuThich>>();
builder.Services.AddScoped<IAllRepo<ThongTinGiaoHang>, AllRepo<ThongTinGiaoHang>>();
builder.Services.AddScoped<IAllRepo<ThuongHieu>, AllRepo<ThuongHieu>>();
builder.Services.AddScoped<IAllRepo<Voucher>, AllRepo<Voucher>>();
builder.Services.AddScoped<IAllRepo<VoucherNguoiDung>, AllRepo<VoucherNguoiDung>>();
builder.Services.AddScoped<IAllRepo<XuatXu>, AllRepo<XuatXu>>();
builder.Services.AddScoped<IAllRepo<KhuyenMai>, AllRepo<KhuyenMai>>();
builder.Services.AddScoped<IAllRepo<KhuyenMaiChiTiet>, AllRepo<KhuyenMaiChiTiet>>();
builder.Services.AddScoped<ISanPhamChiTietRespo, SanPhamChiTietRespo>();
builder.Services.AddScoped<IXuatXuRespo, XuatXuRespo>();
builder.Services.AddScoped<IMauSacRespo, MauSacRespo>();
builder.Services.AddScoped<IAnhRespo, AnhRespo>();
builder.Services.AddScoped<AnhController>();
builder.Services.AddScoped<IAllRepo<DanhGia>, AllRepo<DanhGia>>();
builder.Services.AddScoped<IDanhGiaRepo, DanhGiaRepo>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "bazaizai");
    options.RoutePrefix = string.Empty;
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

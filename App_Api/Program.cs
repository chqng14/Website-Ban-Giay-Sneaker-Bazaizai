using App_Api.Authentication;
using App_Api.Controllers;
using App_Api.Helpers.CustomJson;
using App_Api.Storage;
using App_Data.DbContext;
using App_Data.IRepositories;
using App_Data.Models;
using App_Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure database connection with DATABASE_URL support
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") 
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Database connection is not configured. Set DATABASE_URL or ConnectionStrings:DefaultConnection.");
}

builder.Services.AddDbContext<BazaizaiContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = new CustomJsonNamingPolicy();
});
var internalApiKey = builder.Configuration["ApiSecurity:InternalKey"];
if (string.IsNullOrWhiteSpace(internalApiKey))
{
    throw new InvalidOperationException(
        "Internal API authentication is not configured. Set ApiSecurity:InternalKey.");
}

builder.Services
    .AddAuthentication(InternalApiKeyAuthenticationDefaults.Scheme)
    .AddScheme<InternalApiKeyAuthenticationOptions, InternalApiKeyAuthenticationHandler>(
        InternalApiKeyAuthenticationDefaults.Scheme,
        options => options.ApiKey = internalApiKey);
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder(
            InternalApiKeyAuthenticationDefaults.Scheme)
        .RequireAuthenticatedUser()
        .Build();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        InternalApiKeyAuthenticationDefaults.Scheme,
        new OpenApiSecurityScheme
        {
            Name = InternalApiKeyAuthenticationDefaults.HeaderName,
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Description = "Internal API key used by App_View."
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        [new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = InternalApiKeyAuthenticationDefaults.Scheme
            }
        }] = Array.Empty<string>()
    });
});
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
builder.Services.AddScoped<ISanPhamChiTietRespo, SanPhamChiTietRespo>();
builder.Services.AddScoped<IXuatXuRespo, XuatXuRespo>();
builder.Services.AddScoped<IMauSacRespo, MauSacRespo>();
builder.Services.AddScoped<IAnhRespo, AnhRespo>();
builder.Services.AddScoped<AnhController>();
builder.Services.AddScoped<IAllRepo<DanhGia>, AllRepo<DanhGia>>();
builder.Services.AddScoped<IDanhGiaRepo, DanhGiaRepo>();
builder.Services.AddScoped<IHoaDonRepos, HoaDonRepos>();
builder.Services.AddScoped<IHoaDonChiTietRepos, HoaDonChiTietRepos>();
builder.Services.AddScoped<IKhachHangRepo, KhachHangRepo>();
builder.Services.AddScoped<IVoucherNguoiDungRepos, VoucherNguoiDungRepos>();
builder.Services.AddScoped<IThongTinGHRepos, ThongTinGHRepos>();
builder.Services.AddScoped<IGioHangChiTietRepos, GioHangChiTietRepos>();
builder.Services.AddScoped<HoaDonChiTietController>();
builder.Services.AddScoped<PTThanhToanController>();
builder.Services.AddScoped<PTThanhToanChiTietController>();
builder.Services.AddScoped<DanhGiaController>();
builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.SectionName));
builder.Services.AddSingleton<IImageStorage, LocalImageStorage>();
builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024);
var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BazaizaiContext>();
    await dbContext.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new
        {
            status = "Error",
            message = "An unexpected server error occurred."
        });
    });
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "bazaizai");
        options.RoutePrefix = string.Empty;
    });
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

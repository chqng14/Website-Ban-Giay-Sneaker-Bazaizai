
using App_View.IServices;
using App_Data.DbContextt;
using App_Data.Models;
using App_View.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Security.Claims;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Authentication;
using Google;
using App_View.Models;
using Microsoft.Extensions.Hosting;

using App_View.Controllers;
using Hangfire;
using App_View.Models.Momo;
using App_View.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
// Add services to the container.
//BAZAIZAI\SQLEXPRESS

builder.Services.AddHangfire(x => x.UseSqlServerStorage(@"Data Source=MSI;Initial Catalog=DuAnTotNghiep_BazaizaiStore;Integrated Security=True")); //Đoạn này ai chạy lỗi thì đổi đường dẫn trong này nha



builder.Services.AddHangfireServer();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BazaizaiContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddOptions();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews(); builder.Services.AddScoped<ISanPhamChiTietService, SanPhamChiTietService>();
builder.Services.AddScoped<IVoucherServices, VoucherServices>();
builder.Services.AddScoped<IVoucherNguoiDungServices, VoucherNguoiDungServices>();

builder.Services.AddControllersWithViews(); builder.Services.AddScoped<ISanPhamChiTietService, SanPhamChiTietService>();
builder.Services.AddScoped<IGioHangChiTietServices, GioHangChiTietServices>();
builder.Services.AddScoped<IKhuyenMaiChiTietServices, KhuyenMaiChiTietServices>();
builder.Services.AddScoped<IKhuyenMaiServices, KhuyenMaiServices>();
builder.Services.AddScoped<ThongTinGHController>();  // Sử dụng AddScoped nếu bạn muốn một instance cho mỗi phạm vi của yêu cầu HTTP
builder.Services.AddScoped<GioHangChiTietsController, GioHangChiTietsController>();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
builder.Services.AddScoped<IDanhGiaService, DanhGiaService>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7038/") });
//Thêm
builder.Services.AddIdentity<NguoiDung, ChucVu>()
.AddEntityFrameworkStores<BazaizaiContext>()
.AddDefaultTokenProviders();

var mailsetting = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailsetting);
builder.Services.AddSingleton<IEmailSender, SendMailService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.Configure<IdentityOptions>(options =>
{

    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 6; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Khóa 1 phút
    options.Lockout.MaxFailedAccessAttempts = 100; // Thất bại 5 lần thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_.";

    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    //Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;// sau khi đăng kí....(tự hiểu)

});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.LoginPath = "/Login/";
    options.LogoutPath = "/Lockout/";
    options.AccessDeniedPath = "/KhongDuocTruyCap.html";
});
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.ExpireTimeSpan = TimeSpan.FromDays(14);
//    options.LoginPath = "/Login/";
//    options.LogoutPath = "/Lockout/";
//    options.AccessDeniedPath = "/KhongDuocTruyCap.html";
//    options.ReturnUrlParameter = "/Admin/";
//    options.Events = new CookieAuthenticationEvents
//    {
//        OnValidatePrincipal = context =>
//        {

//            Kiểm tra vai trò người dùng
//            var userRoles = context.Principal.Claims
//                .Where(c => c.Type == ClaimTypes.Role)
//                .Select(c => c.Value)
//                .ToList();

//            Xác định thời gian hết hạn dựa trên vai trò
//            var expireTimeSpan = userRoles.Contains("Admin")

//                ? TimeSpan.FromDays(14)  // Thời gian hết hạn cho vai trò Admin là 30 ngày
//                : userRoles.Contains("NhanVien") ? TimeSpan.FromDays(14) : TimeSpan.FromMinutes(1); // Mặc định cho các vai trò khác là 14 ngày

//            Cập nhật thời gian hết hạn
//            context.Properties.ExpiresUtc = DateTimeOffset.UtcNow.Add(expireTimeSpan);

//            return Task.CompletedTask;

//        },
//        OnRedirectToReturnUrl = context =>
//        {
//            var userRoles = context.HttpContext.User.Claims
//                .Where(c => c.Type == ClaimTypes.Role)
//                .Select(c => c.Value)
//                .ToList();

//            Kiểm tra vai trò và chuyển hướng tương ứng
//            if (userRoles.Contains("Admin"))
//            {
//                context.RedirectUri = "/admin";
//            }
//            else if (userRoles.Contains("NhanVien"))
//            {
//                context.RedirectUri = "/admin";
//            }
//            else
//            {
//                context.RedirectUri = "/Index";
//            }

//            return Task.CompletedTask;
//        }
//    };
//});
builder.Services.AddAuthentication()
     .AddCookie()
    .AddGoogle(googleOptions =>
    {
        // Đọc thông tin Authentication:Google từ appsettings.json
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");

        // Thiết lập ClientID và ClientSecret để truy cập API google
        googleOptions.ClientId = googleAuthNSection["ClientId"];
        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
        //googleOptions.AccessDeniedPath = "/login/";
        //googleOptions.Scope.Add("https://www.googleapis.com/auth/user.birthday.read");
        googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        googleOptions.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
        googleOptions.SaveTokens = true;
    })
    .AddFacebook(facebookOptions =>
    {
        // Đọc cấu hình
        IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
        facebookOptions.AppId = facebookAuthNSection["AppId"];
        facebookOptions.AppSecret = facebookAuthNSection["AppSecret"];

    });
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();
builder.Services.Configure<SecurityStampValidatorOptions>(option =>
{
    option.ValidationInterval = TimeSpan.FromSeconds(1);

});
builder.Services.AddSession(Options =>
{
    Options.IdleTimeout = TimeSpan.FromDays(20);
});
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("TwilioSettings"));
builder.Services.AddScoped<ISMSSenderService, SMSSenderService>();
//thêm
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<BazaizaiContext>();
        var userManager = services.GetRequiredService<UserManager<NguoiDung>>();
        var roleManager = services.GetRequiredService<RoleManager<ChucVu>>();
        await ContextdDefault.SeedRolesAsync(userManager, roleManager);
        await ContextdDefault.SeeAdminAsync(userManager, roleManager);
        await ContextdDefault.PhuongThucThanhToan();

    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard();
var capNhatTime = new CapNhatThoiGianService();
Task.Run(() =>
{
    while (true)
    {
        capNhatTime.CheckNgayKetThuc();
        capNhatTime.CapNhatTrangThaiSaleDetail();
        capNhatTime.CapNhatGiaBanThucTe();
        capNhatTime.CapNhatVoucherHetHanOnline();
        capNhatTime.CapNhatVoucherDenHanOnline();
        capNhatTime.CapNhatVoucherNguoiDungOnline();
        capNhatTime.CapNhatVoucherHetHanTaiQuay();
        capNhatTime.CapNhatVoucherDenHanTaiQuay();
        capNhatTime.CapNhatVoucherNguoiDungTaiQuay();
        capNhatTime.CapNhatVoucherNguoiDungTaiQuayKhiVoucherHoatDong();
        Thread.Sleep(TimeSpan.FromSeconds(5));
    }
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapRazorPages();
app.MapControllers();
app.Run();

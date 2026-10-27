using App_View.IServices;
using App_Data.DbContext;
using App_Data.Models;
using App_Data.IRepositories;
using App_Data.Repositories;
using App_View.Controllers;
using App_View.Models;
using App_View.Models.Momo;
using App_View.Services;
using App_View.Settings;
using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Database connection is not configured. Set DATABASE_URL or ConnectionStrings:DefaultConnection.");
}
builder.Services.AddHangfire(x => x.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();
builder.Services.AddDbContext<BazaizaiContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddOptions();
builder.Services.AddRazorPages();
builder.Services.AddScoped<ISanPhamChiTietservice, SanPhamChiTietservice>();
builder.Services.AddScoped<IVoucherservices, Voucherservices>();
builder.Services.AddScoped<IVoucherNguoiDungservices, VoucherNguoiDungservices>();
builder.Services.AddScoped<IGioHangChiTietservices, GioHangChiTietservices>();
builder.Services.AddScoped<IKhuyenMaiChiTietservices, KhuyenMaiChiTietservices>();
builder.Services.AddScoped<IKhuyenMaiservices, KhuyenMaiservices>();
builder.Services.AddScoped<ThongTinGHController>();
builder.Services.AddScoped<GioHangChiTietsController, GioHangChiTietsController>();
builder.Services.AddScoped<IViewRenderService, ViewRenderService>();
builder.Services.AddScoped<IDanhGiaservice, DanhGiaservice>();
builder.Services.AddScoped<IHoaDonServices, HoaDonServices>();
builder.Services.AddScoped<IThongKeService, ThongKeService>();
builder.Services.AddScoped<IThongTinGHServices, ThongTinGHServices>();
builder.Services.AddScoped<IHoaDonChiTietservices, HoaDonChiTietservices>();
builder.Services.AddScoped<IPTThanhToanServices, PTThanhToanServices>();
builder.Services.AddScoped<IPTThanhToanChiTietServices, PTThanhToanChiTietServices>();
builder.Services.AddScoped<IAllRepo<KhuyenMaiChiTiet>, AllRepo<KhuyenMaiChiTiet>>();
builder.Services.AddScoped<PTThanhToanController>();
builder.Services.AddScoped<PTThanhToanChiTietController>();
builder.Services.AddScoped<CapNhatThoiGianService>();
builder.Services.AddSingleton<IUserImageStorage, LocalUserImageStorage>();
builder.Services.AddHostedService<PromotionUpdateBackgroundService>();
var configuredKeyPath = builder.Configuration["DataProtection:KeysPath"] ?? "keys";
var dataProtectionKeyPath = Path.GetFullPath(Path.IsPathRooted(configuredKeyPath)
    ? configuredKeyPath
    : Path.Combine(builder.Environment.ContentRootPath, configuredKeyPath));
Directory.CreateDirectory(dataProtectionKeyPath);
builder.Services.AddDataProtection()
    .SetApplicationName("BazaizaiStore")
    .PersistKeysToFileSystem(new DirectoryInfo(dataProtectionKeyPath));
var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7038/";
if (!Uri.TryCreate(apiBaseUrl, UriKind.Absolute, out var apiBaseUri))
{
    throw new InvalidOperationException("ApiSettings:BaseUrl must be a valid absolute URL.");
}
builder.Services.AddHttpClient("BazaizaiApi", client =>
{
    client.BaseAddress = apiBaseUri;
    client.Timeout = TimeSpan.FromSeconds(30);
    var internalApiKey = builder.Configuration["ApiSecurity:InternalKey"];
    if (!string.IsNullOrWhiteSpace(internalApiKey))
    {
        client.DefaultRequestHeaders.Add("X-Internal-Api-Key", internalApiKey);
    }
});
builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("BazaizaiApi"));
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
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 5; // Thất bại 5 lần thì khóa
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
var authenticationBuilder = builder.Services.AddAuthentication();
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
{
    authenticationBuilder.AddGoogle(googleOptions =>
    {
        // Đọc thông tin Authentication:Google từ appsettings.json
        // Thiết lập ClientID và ClientSecret để truy cập API google
        googleOptions.ClientId = googleClientId;
        googleOptions.ClientSecret = googleClientSecret;
        //googleOptions.AccessDeniedPath = "/login/";
        //googleOptions.Scope.Add("https://www.googleapis.com/auth/user.birthday.read");
        googleOptions.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
        googleOptions.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
        googleOptions.SaveTokens = true;
    });
}

var facebookAppId = builder.Configuration["Authentication:Facebook:AppId"];
var facebookAppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
if (!string.IsNullOrWhiteSpace(facebookAppId) && !string.IsNullOrWhiteSpace(facebookAppSecret))
{
    authenticationBuilder.AddFacebook(facebookOptions =>
    {
        // Đọc cấu hình
        facebookOptions.AppId = facebookAppId;
        facebookOptions.AppSecret = facebookAppSecret;

    });
}
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
        await ContextdDefault.SeedAdminAsync(userManager, builder.Configuration);
        await ContextdDefault.PhuongThucThanhToan(
            services.GetRequiredService<IPTThanhToanServices>());

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

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles();
var configuredStorageRoot = builder.Configuration["Storage:RootPath"] ?? "storage";
var storageRoot = Path.GetFullPath(Path.IsPathRooted(configuredStorageRoot)
    ? configuredStorageRoot
    : Path.Combine(app.Environment.ContentRootPath, configuredStorageRoot));
Directory.CreateDirectory(storageRoot);
foreach (var publicFolder in new[] { "AnhSanPham", "AnhSale", "images", "user_img" })
{
    var publicDirectory = Path.Combine(storageRoot, publicFolder);
    Directory.CreateDirectory(publicDirectory);
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(publicDirectory),
        RequestPath = $"/{publicFolder}"
    });
}
app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
if (app.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
}
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

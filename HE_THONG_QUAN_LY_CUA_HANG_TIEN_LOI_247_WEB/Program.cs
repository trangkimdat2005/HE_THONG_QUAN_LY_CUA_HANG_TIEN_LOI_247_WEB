using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.EF;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Models.Entities;
using HE_THONG_QUAN_LY_CUA_HANG_TIEN_LOI_247_WEB.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;  // Ð?m b?o session cookie là thi?t y?u
    options.Cookie.HttpOnly = true;     // Ch? cho phép cookie ðý?c truy c?p qua HTTP (không cho phép JavaScript truy c?p cookie)
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Ch? g?i cookie qua HTTPS
    options.IdleTimeout = TimeSpan.FromHours(3);  // Th?i gian h?t h?n session
    options.Cookie.Name = "QuanLyCuaHangTienLoi";  // Ð?t tên cookie cho session
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // C?u h?nh SameSite ð? h? tr? cross-origin requests
        options.Cookie.SameSite = SameSiteMode.Lax;  // Có th? s? d?ng SameSiteMode.Strict n?u b?n mu?n ch? cho phép cookie trong cùng domain
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Ð?m b?o cookie ch? ðý?c g?i qua HTTPS
        options.ExpireTimeSpan = TimeSpan.FromHours(3);  // Th?i gian h?t h?n c?a cookie
        options.LoginPath = "/LoiDangNhap/LoiDangNhap";  // Ðý?ng d?n ðãng nh?p
        options.AccessDeniedPath = "/LoiDangNhap/LoiDangNhap";  // Ðý?ng d?n khi b? t? ch?i quy?n
        options.SlidingExpiration = true;  // Gia h?n cookie khi ngý?i dùng týõng tác
    });

// C?u h?nh CookiePolicy ð? yêu c?u s? ð?ng ? t? ngý?i dùng cho các cookie không thi?t y?u
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;  // Yêu c?u s? ð?ng ? c?a ngý?i dùng cho các cookie không thi?t y?u
    options.MinimumSameSitePolicy = SameSiteMode.Lax;  // SameSitePolicy phù h?p cho cross-origin requests
});



builder.Services.AddSignalR();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);



builder.Services.AddScoped<IQuanLyServices, QuanLyServices>();
builder.Services.AddScoped<IDashboardServices, DashboardServices>();
builder.Services.AddScoped<ITonKhoServices, TonKhoServices>();
builder.Services.AddScoped<IGiaoDichThanhToanServices, GiaoDichThanhToanServices>();
builder.Services.AddScoped<IPhieuDoiTraServices, PhieuDoiTraServices>();
builder.Services.AddScoped<IChinhSachHoanTraServices, ChinhSachHoanTraServices>();
builder.Services.AddScoped<IRealtimeNotifier, RealtimeNotifier>();
builder.Services.AddScoped<IPermissionServices, PermissionServices>();
// ??ng ký Email Service
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddAuthorization(options =>
{
    // L?y danh sách quy?n t? d?ch v?
    var permissions = builder.Services.BuildServiceProvider().GetRequiredService<IQuanLyServices>().GetList<Permission>();

    foreach (var permission in permissions)
    {
        // Ðãng k? policy cho m?i quy?n trong cõ s? d? li?u
        options.AddPolicy(permission.Code, policy =>
            policy.RequireClaim("Permission", permission.Code));
    }
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapHub<ReloadHub>("/hubs/reload");

app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=HomeAdmin}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();

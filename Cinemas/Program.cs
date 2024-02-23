using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Notyf.Models;
using Cinemas.Models;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Unicode;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
builder.Services.AddNotyf(config => {config.DurationInSeconds = 10; config.IsDismissable =true; config.Position = NotyfPosition.TopRight;});
builder.Services.AddDbContext<DbCinemasContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("dbCinemas")));
builder.Services.AddSession(cfg => {                // Đăng ký dịch vụ Session
    cfg.IdleTimeout = new TimeSpan(0, 120, 0);       // Thời gian tồn tại của Session
});

builder.Services.AddAuthentication("CookieAuthentication").AddCookie("CookieAuthentication", config =>
{
    config.Cookie.Name = "UserLoginCookie";
    config.ExpireTimeSpan = TimeSpan.FromDays(1);
    config.LoginPath = "/dang-nhap.html";
    config.LogoutPath = "/dang-xuat.html";
    config.AccessDeniedPath = "/not-found.html";
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.Expiration = TimeSpan.FromDays(150);
    options.ExpireTimeSpan = TimeSpan.FromDays(150);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
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
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Phim}/{action=Index}/{id?}");

app.Run();

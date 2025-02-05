using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ProductService>(); // ProductService eklenmiþ
builder.Services.AddScoped<AssigmentService>(); // ProductService eklenmiþ
builder.Services.AddScoped<JoyStaffService>();
builder.Services.AddScoped<ServerService>();
builder.Services.AddScoped<TeamService>();
builder.Services.AddScoped<LicenceService>();
builder.Services.AddScoped<EmailService>();

// Configure RelatedDigitalEmailSettings from appsettings.json
builder.Services.Configure<RelatedDigitalEmailSettings>(builder.Configuration.GetSection("RelatedDigitalEmailSettings"));
builder.Services.AddHttpClient<EmailService>();


builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        // Varsayýlan view yollarýný temizliyoruz.
        options.ViewLocationFormats.Clear();

        // Özelleþtirilmiþ view yollarýný ekliyoruz.
        options.ViewLocationFormats.Add("/Web/Views/{1}/{0}.cshtml"); // Controller ve view ismiyle eþleþen view'lar
        options.ViewLocationFormats.Add("/Web/Views/Shared/{0}.cshtml");  // Shared view'lar
    });

builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:Dbconnection"]));

builder.Services.AddIdentity<JoyUser, JoyRole>(options =>
{
    // Kullanýcý oturum açma süresi
    options.SignIn.RequireConfirmedAccount = false;  // E-posta onayý zorunlu deðilse
})
.AddEntityFrameworkStores<InventoryContext>()
.AddDefaultTokenProviders();

// Cookie ayarlarýný yapýlandýrýyoruz
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";  // Giriþ yapmamýþ kullanýcýyý yönlendirecek yol
    options.LogoutPath = "/User/Logout"; // Çýkýþ yaptýktan sonra yönlendirecek yol
    options.AccessDeniedPath = "/User/AccessDenied"; // Eriþim engellendiðinde yönlendirilecek yol
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Oturum süresi 30 dakika
    options.SlidingExpiration = true; // Oturum süresi her etkinlikte uzar
    options.Cookie.HttpOnly = true; // Cookie yalnýzca sunucu tarafýndan eriþilebilir
    options.Cookie.SameSite = SameSiteMode.Strict; // Güvenlik için SameSite seçeneði
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

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

IdentitySeedData.IdentityTestUser(app);

app.Run();

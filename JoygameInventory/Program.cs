using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ProductService>(); // ProductService eklenmi�
builder.Services.AddScoped<AssigmentService>(); // ProductService eklenmi�
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
        // Varsay�lan view yollar�n� temizliyoruz.
        options.ViewLocationFormats.Clear();

        // �zelle�tirilmi� view yollar�n� ekliyoruz.
        options.ViewLocationFormats.Add("/Web/Views/{1}/{0}.cshtml"); // Controller ve view ismiyle e�le�en view'lar
        options.ViewLocationFormats.Add("/Web/Views/Shared/{0}.cshtml");  // Shared view'lar
    });

builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:Dbconnection"]));

builder.Services.AddIdentity<JoyUser, JoyRole>(options =>
{
    // Kullan�c� oturum a�ma s�resi
    options.SignIn.RequireConfirmedAccount = false;  // E-posta onay� zorunlu de�ilse
})
.AddEntityFrameworkStores<InventoryContext>()
.AddDefaultTokenProviders();

// Cookie ayarlar�n� yap�land�r�yoruz
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";  // Giri� yapmam�� kullan�c�y� y�nlendirecek yol
    options.LogoutPath = "/User/Logout"; // ��k�� yapt�ktan sonra y�nlendirecek yol
    options.AccessDeniedPath = "/User/AccessDenied"; // Eri�im engellendi�inde y�nlendirilecek yol
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Oturum s�resi 30 dakika
    options.SlidingExpiration = true; // Oturum s�resi her etkinlikte uzar
    options.Cookie.HttpOnly = true; // Cookie yaln�zca sunucu taraf�ndan eri�ilebilir
    options.Cookie.SameSite = SameSiteMode.Strict; // G�venlik i�in SameSite se�ene�i
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

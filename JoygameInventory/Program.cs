using JoygameInventory.Business.Interface;
using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using JoygameInventory.Models.Model;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IAssigmentService,AssigmentService>(); 
builder.Services.AddScoped<IJoyStaffService,JoyStaffService>();
builder.Services.AddScoped<ITeamService,TeamService>();
builder.Services.AddScoped<ILicenceService,LicenceService>();
builder.Services.AddScoped<IMaintenanceService,MaintenanceService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


//Email servisi i�in gerekli ayarlar� yap�land�r�yoruz
builder.Services.Configure<RelatedDigitalEmailSettings>(builder.Configuration.GetSection("RelatedDigitalEmailSettings"));
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<EmailService>();
builder.Services.AddSingleton<TokenStorage>();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlite(builder.Configuration["ConnectionStrings:Dbconnection"]));


// Kullan�c� Identity ayarlar�n� yap�land�r�yoruz
builder.Services.AddIdentity<JoyUser, JoyRole>(options =>
{
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



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ProductManagement}/{action=ProductList}/{id?}");

IdentitySeedData.IdentityTestUser(app);

app.Run();

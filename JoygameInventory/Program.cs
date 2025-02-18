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

builder.Services.AddScoped<IProductService,ProductService>(); // ProductService eklenmi�
builder.Services.AddScoped<IAssigmentService,AssigmentService>(); // AssigmentService eklenmi�
builder.Services.AddScoped<IJoyStaffService,JoyStaffService>();
builder.Services.AddScoped<ITeamService,TeamService>();
builder.Services.AddScoped<ILicenceService,LicenceService>();
builder.Services.AddScoped<IMaintenanceService,MaintenanceService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


builder.Services.Configure<RelatedDigitalEmailSettings>(builder.Configuration.GetSection("RelatedDigitalEmailSettings"));
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<EmailService>();// EmailService eklenmi�
builder.Services.AddSingleton<TokenStorage>();


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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ProductManagement}/{action=ProductList}/{id?}");

IdentitySeedData.IdentityTestUser(app);

app.Run();

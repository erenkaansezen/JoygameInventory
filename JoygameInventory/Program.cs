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

builder.Services.AddScoped<IProductService,ProductService>(); // ProductService eklenmiþ
builder.Services.AddScoped<IAssigmentService,AssigmentService>(); // AssigmentService eklenmiþ
builder.Services.AddScoped<IJoyStaffService,JoyStaffService>();
builder.Services.AddScoped<ITeamService,TeamService>();
builder.Services.AddScoped<ILicenceService,LicenceService>();
builder.Services.AddScoped<IMaintenanceService,MaintenanceService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


builder.Services.Configure<RelatedDigitalEmailSettings>(builder.Configuration.GetSection("RelatedDigitalEmailSettings"));
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<EmailService>();// EmailService eklenmiþ
builder.Services.AddSingleton<TokenStorage>();


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

using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ProductService>(); // ProductService eklenmiþ
builder.Services.AddScoped<AssigmentService>(); // ProductService eklenmiþ
builder.Services.AddScoped<JoyStaffService>();


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

builder.Services.AddIdentity<JoyUser, JoyRole>()
    .AddEntityFrameworkStores<InventoryContext>()
    .AddDefaultTokenProviders();

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

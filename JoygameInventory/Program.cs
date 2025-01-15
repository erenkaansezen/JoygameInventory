using JoygameInventory.Business.Services;
using JoygameInventory.Data.Context;
using JoygameInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ProductService>(); // ProductService eklenmi�
builder.Services.AddScoped<AssigmentService>(); // ProductService eklenmi�
builder.Services.AddScoped<JoyStaffService>();


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

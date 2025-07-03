using HealthBridges.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<HealthBridgeDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ConnectionString1"),
    new MySqlServerVersion(new Version(8, 0, 21))));


builder.Services.AddDbContext<HealthBridgeDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("ConnectionString1"),
                     ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ConnectionString1"))));


//builder.Services.AddDbContext<HealthBridgeDbContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("ConnectionString1")));


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

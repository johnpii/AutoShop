using AutoShop.Data;
using AutoShop.Helpers;
using AutoShop.Hubs;
using AutoShop.Interfaces;
using AutoShop.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = ConfigurationHelper.expireTimeCookie;
    });
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSingleton(new MongoClient(builder.Configuration.GetSection("MongoDB:ConnectionString").Value).GetDatabase(builder.Configuration.GetSection("MongoDB:DatabaseName").Value));
builder.Services.AddMvc();

builder.Services.AddSignalR();

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = ConfigurationHelper.expireTimeCookie;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
ConfigurationHelper.Initialize(builder.Configuration);
builder.Services.AddScoped<IAutoRepository, AutoRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = "localhost";
//    options.InstanceName = "local";
//});

var app = builder.Build();
app.UseResponseCompression();
app.UseSession();

app.UseAuthentication();
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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<AutoShopHub>("/AutoShopHub");

app.Run();

public partial class Program { }
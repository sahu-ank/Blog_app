using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using mvc.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<mvcContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("mvcContext") ?? throw new InvalidOperationException("Connection string 'mvcContext' not found.")));

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Blop_app.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(1200);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.Cookie.Name = ".Blop_app.Cookie";
    options.LoginPath = "/Auth/Login";
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("hr"));
    options.AddPolicy("Employee", policy => policy.RequireRole("employee"));
});

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

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
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.Run();

using JobSeeker.Data;
using JobSeeker.Middleware;
using JobSeeker.Services.FileStorageService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Menambahkan layanan untuk IFileStorageService
var storagePath = Path.Combine(builder.Environment.WebRootPath, "storage");
builder.Services.AddSingleton<IFileStorageService>(provider =>
    new LocalFileStorageService(storagePath));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database Integration Using EF Core (Using for Migration)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 26)))); // Ganti versi sesuai dengan yang Anda gunakan

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Waktu kedaluwarsa Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Tambahkan IHttpContextAccessor
builder.Services.AddHttpContextAccessor();
// Tambahkan konfigurasi untuk session
builder.Services.AddSession();

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
app.UseAuthorization();
app.UseHttpMethodOverride();

// Custom Middleware
app.UseMiddleware<AuthenticationMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

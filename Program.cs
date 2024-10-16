var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Waktu kedaluwarsa Session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
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

app.UseSession();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    // Daftar URL yang tidak memerlukan autentikasi
    var allowedPaths = new[] { "/Auth/Login", "/Auth/Register", "/Auth/ForgotPassword" };

    // Cek apakah URL saat ini ada dalam daftar yang diizinkan
    if (allowedPaths.Contains(context.Request.Path.ToString(), StringComparer.OrdinalIgnoreCase))
    {
        // Cek apakah pengguna sudah terautentikasi saat mencoba mengakses halaman login
        if (context.Session.TryGetValue("UserId", out _))
        {
            context.Response.Redirect("/Home"); // Redirect ke halaman utama jika sudah terautentikasi
            return; // Hentikan eksekusi lebih lanjut
        }
    }
    else
    {
        // Jika tidak terautentikasi, redirect ke halaman login
        if (!context.Session.TryGetValue("UserId", out _))
        {
            context.Response.Redirect("/Auth/Login");
            return; // Hentikan eksekusi lebih lanjut
        }
    }

    await next(); // Lanjutkan ke middleware berikutnya jika terautentikasi
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

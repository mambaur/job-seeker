
namespace JobSeeker.Middleware
{
    public class AuthenticationMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // Daftar URL yang tidak memerlukan autentikasi
            var allowedPaths = new[] { "/Auth/Login", "/Auth/Register", "/Auth/ForgotPassword", "/Auth/ChangePassword" };

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

            await _next(context); // Lanjutkan ke middleware berikutnya jika terautentikasi
        }
    }
}
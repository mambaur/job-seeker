using JobSeeker.Models;
using JobSeeker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class AuthController : Controller
    {
        private AuthRepository authRepository = new AuthRepository();

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            var user = authRepository.AuthenticateUser(username, password);
            Console.WriteLine($"Ini adalah data user {user}");
            if (user != null)
            {
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetString("Username", user.Username!);
                // Session["UserId"] = user.Id;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = "Invalid username or password";
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            var existingUser = authRepository.GetUserByUsername(user.Username ?? "");
            if (existingUser != null)
            {
                ViewBag.Message = "Username already taken";
                return View();
            }


            ViewBag.Message = "Register success";
            authRepository.RegisterUser(user);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
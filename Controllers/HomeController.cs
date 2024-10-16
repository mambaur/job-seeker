using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JobSeeker.Models;
using JobSeeker.Repositories;

namespace JobSeeker.Controllers;

public class HomeController : Controller
{

    private AuthRepository authRepository = new AuthRepository();

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.TryGetValue("Username", out _))
        {
            var existingUser = authRepository.GetUserByUsername(HttpContext.Session.GetString("Username") ?? "");
            if (existingUser != null)
            {
                return View(existingUser);
            }
        }

        return RedirectToAction("Login");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

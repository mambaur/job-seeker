using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JobSeeker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobSeeker.Controllers
{
    public class ApplicantController : Controller
    {
        private AuthRepository authRepository = new AuthRepository();
        
        private readonly ILogger<ApplicantController> _logger;

        public ApplicantController(ILogger<ApplicantController> logger)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
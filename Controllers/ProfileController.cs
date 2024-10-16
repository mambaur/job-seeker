using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JobSeeker.Models;
using JobSeeker.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobSeeker.Controllers
{
    public class ProfileController : Controller
    {
        private AuthRepository authRepository = new AuthRepository();
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(ILogger<ProfileController> logger)
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

        [HttpPost]
        public async Task<IActionResult> Update(User user)
        {
            if (ModelState.IsValid)
            {
                var isUpdated = await authRepository.UpdateUserAsync(user);
                if (isUpdated)
                {
                    return RedirectToAction("Index", "Profile");
                }
                ModelState.AddModelError("", "Failed to update user.");
            }

            return RedirectToAction("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(int userId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                ModelState.AddModelError("", "Password tidak boleh kosong.");
                return RedirectToAction("Index", "Profile");
            }

            bool isUpdated = await authRepository.ChangePasswordAsync(userId, newPassword);
            if (isUpdated)
            {
                return RedirectToAction("Index", "Profile");
            }
            else
            {
                ModelState.AddModelError("", "Gagal mengubah password.");
                return RedirectToAction("Index", "Profile");
            }
        }
    }

}
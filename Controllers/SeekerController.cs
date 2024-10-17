using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using JobSeeker.Models;
using JobSeeker.Repositories;
using JobSeeker.Services.FileStorageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobSeeker.Controllers
{
    public class SeekerController : Controller
    {
        private readonly ILogger<SeekerController> _logger;
        private readonly SeekerRepository seekerRepository = new SeekerRepository();
        private readonly IFileStorageService _fileStorageService;

        public SeekerController(ILogger<SeekerController> logger, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public IActionResult Index()
        {
            var Seekers = seekerRepository.GetAllSeekers();
            return View(Seekers);
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store(User user, IFormFile ImageFile)
        {
            User User = new()
            {
                Name = user.Name,
                Email = user.Email,
                Username = user.Username,
            };

            if (ImageFile != null && ImageFile.Length != 0)
            {
                var FileName = await _fileStorageService.SaveFileAsync(ImageFile);
                User.ImageUrl = FileName;
            }

            seekerRepository.Store(User);

            ViewBag.Message = "Create new seeker success";
            return RedirectToAction("Index", "Seeker");
        }

        public IActionResult Edit(int Id)
        {
            var Seeker = seekerRepository.GetSeekerById(Id);
            return View(Seeker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(User user, IFormFile ImageFile)
        {
            User? User = seekerRepository.GetSeekerById(user.Id);

            if (User != null)
            {
                User UserUpdate = new()
                {
                    Id = User.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Username = user.Username,
                    ImageUrl = User.ImageUrl
                };

                if (ImageFile != null && ImageFile.Length != 0)
                {
                    var FileName = await _fileStorageService.SaveFileAsync(ImageFile);
                    UserUpdate.ImageUrl = FileName;
                }

                var isUpdated = seekerRepository.Update(UserUpdate);
                if (isUpdated)
                {
                    if (User?.ImageUrl != null && ImageFile != null)
                    {
                        await _fileStorageService.DeleteFileAsync(User.ImageUrl);
                    }

                    return RedirectToAction("Index", "Seeker");
                }
                ModelState.AddModelError("", "Failed to update seeker.");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {

            User? User = seekerRepository.GetSeekerById(Id);

            if (User != null && User?.ImageUrl != null)
            {
                await _fileStorageService.DeleteFileAsync(User.ImageUrl);
            }

            seekerRepository.DeleteSeekerById(Id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
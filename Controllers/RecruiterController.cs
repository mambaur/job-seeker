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
    public class RecruiterController : Controller
    {
        private readonly ILogger<RecruiterController> _logger;
        private readonly RecruiterRepository recruiterRepository = new RecruiterRepository();
        private readonly IFileStorageService _fileStorageService;

        public RecruiterController(ILogger<RecruiterController> logger, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public IActionResult Index()
        {
            var Recruiters = recruiterRepository.GetAllRecruiters();
            return View(Recruiters);
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

            recruiterRepository.Store(User);

            ViewBag.Message = "Create new recruiter success";
            return RedirectToAction("Index", "Recruiter");
        }

        public IActionResult Edit(int Id)
        {
            var Recruiter = recruiterRepository.GetRecruiterById(Id);
            return View(Recruiter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(User user, IFormFile ImageFile)
        {
            User? User = recruiterRepository.GetRecruiterById(user.Id);

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

                var isUpdated = recruiterRepository.Update(UserUpdate);
                if (isUpdated)
                {
                    if (User?.ImageUrl != null && ImageFile != null)
                    {
                        await _fileStorageService.DeleteFileAsync(User.ImageUrl);
                    }

                    return RedirectToAction("Index", "Recruiter");
                }
                ModelState.AddModelError("", "Failed to update recruiter.");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {

            User? User = recruiterRepository.GetRecruiterById(Id);

            if (User != null && User?.ImageUrl != null)
            {
                await _fileStorageService.DeleteFileAsync(User.ImageUrl);
            }

            recruiterRepository.DeleteRecruiterById(Id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
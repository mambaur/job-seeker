using JobSeeker.Models;
using JobSeeker.Repositories;
using JobSeeker.Services.FileStorageService;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly OrganizationRepository organizationRepository = new OrganizationRepository();
        private readonly IFileStorageService _fileStorageService;

        private readonly ILogger<OrganizationController> _logger;

        public OrganizationController(ILogger<OrganizationController> logger, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public IActionResult Index()
        {
            var Organization = organizationRepository.GetAllOrganizations();
            return View(Organization);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store(string Name, string Description, IFormFile ImageFile)
        {
            Organization Organization = new()
            {
                Name = Name,
                Description = Description
            };

            if (ImageFile != null && ImageFile.Length != 0)
            {
                var FileName = await _fileStorageService.SaveFileAsync(ImageFile);
                Organization.ImageUrl = FileName;
            }

            organizationRepository.Store(Organization);

            ViewBag.Message = "Create new organization success";
            return RedirectToAction("Index", "Organization");
        }

        public IActionResult Edit(int Id)
        {
            var Organization = organizationRepository.GetOrganizationById(Id);
            return View(Organization);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int Id, string Name, string Description, IFormFile ImageFile)
        {
            Organization? Organization = organizationRepository.GetOrganizationById(Id);

            if (Organization != null)
            {
                Organization OrganizationUpdate = new()
                {
                    Id = Organization.Id,
                    Name = Name,
                    Description = Description,
                    ImageUrl = Organization.ImageUrl
                };

                if (ImageFile != null && ImageFile.Length != 0)
                {
                    var FileName = await _fileStorageService.SaveFileAsync(ImageFile);
                    OrganizationUpdate.ImageUrl = FileName;
                }

                var isUpdated = organizationRepository.Update(OrganizationUpdate);
                if (isUpdated)
                {
                    if (Organization?.ImageUrl != null && ImageFile != null)
                    {
                        await _fileStorageService.DeleteFileAsync(Organization.ImageUrl);
                    }

                    return RedirectToAction("Index", "Organization");
                }
                ModelState.AddModelError("", "Failed to update job organization.");
            }
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {

            Organization? Organization = organizationRepository.GetOrganizationById(Id);

            if (Organization != null && Organization?.ImageUrl != null)
            {
                await _fileStorageService.DeleteFileAsync(Organization.ImageUrl);
            }

            organizationRepository.DeleteOrganizationById(Id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
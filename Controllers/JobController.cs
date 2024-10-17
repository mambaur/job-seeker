using JobSeeker.Models;
using JobSeeker.Repositories;
using JobSeeker.Services.FileStorageService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobSeeker.Controllers
{
    public class JobController : Controller
    {
        private readonly ILogger<JobController> _logger;
        private readonly JobRepository jobRepository = new JobRepository();
        private readonly IFileStorageService _fileStorageService;

        public JobController(ILogger<JobController> logger, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public IActionResult Index()
        {
            var Jobs = jobRepository.GetAllJobs();
            return View(Jobs);
        }

        [HttpGet]
        public IActionResult Create()
        {
            JobPositionRepository jobPositionRepository = new JobPositionRepository();
            ViewBag.JobPositions = jobPositionRepository.GetAllJobPositions();

            JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();
            ViewBag.JobCategories = jobCategoryRepository.GetAllJobCategories();

            RecruiterRepository recruiterRepository = new RecruiterRepository();
            ViewBag.Recruiters = recruiterRepository.GetAllRecruiters();

            OrganizationRepository organizationRepository = new OrganizationRepository();
            ViewBag.Organizations = organizationRepository.GetAllOrganizations();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Store(Job job, IFormFile ImageFile)
        {

            if (ImageFile != null && ImageFile.Length != 0)
            {
                var FileName = await _fileStorageService.SaveFileAsync(ImageFile);
                job.ImageUrl = FileName;
            }

            jobRepository.Store(job);

            ViewBag.Message = "Create new job success";
            return RedirectToAction("Index", "Job");
        }

        public IActionResult Edit(int Id)
        {
            var Job = jobRepository.GetJobById(Id);

            JobPositionRepository jobPositionRepository = new JobPositionRepository();
            ViewBag.JobPositions = jobPositionRepository.GetAllJobPositions();

            JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();
            ViewBag.JobCategories = jobCategoryRepository.GetAllJobCategories();

            RecruiterRepository recruiterRepository = new RecruiterRepository();
            ViewBag.Recruiters = recruiterRepository.GetAllRecruiters();

            OrganizationRepository organizationRepository = new OrganizationRepository();
            ViewBag.Organizations = organizationRepository.GetAllOrganizations();
            
            return View(Job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Job job, IFormFile ImageFile)
        {
            Job? Job = jobRepository.GetJobById(job.Id);

            if (Job != null)
            {
                Job JobUpdate = new()
                {
                    Id = job.Id,
                    JobCategoryId = job.JobCategoryId,
                    JobPositionId = job.JobPositionId,
                    RecruiterId = job.RecruiterId,
                    OrganizationId = job.OrganizationId,
                    Title = job.Title,
                    Description = job.Description,
                    Status = job.Status,
                    PublishedAt = job.PublishedAt,
                    StartDate = job.StartDate,
                    EndDate = job.EndDate,
                    ImageUrl = Job.ImageUrl
                };

                if (ImageFile != null && ImageFile.Length != 0)
                {
                    var FileName = await _fileStorageService.SaveFileAsync(ImageFile);
                    JobUpdate.ImageUrl = FileName;
                }

                var isUpdated = jobRepository.Update(JobUpdate);
                if (isUpdated)
                {
                    if (Job?.ImageUrl != null && ImageFile != null)
                    {
                        await _fileStorageService.DeleteFileAsync(Job.ImageUrl);
                    }

                    return RedirectToAction("Index", "Job");
                }
                ModelState.AddModelError("", "Failed to update job.");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {

            Job? Job = jobRepository.GetJobById(Id);

            if (Job != null && Job?.ImageUrl != null)
            {
                await _fileStorageService.DeleteFileAsync(Job.ImageUrl);
            }

            jobRepository.DeleteJobById(Id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
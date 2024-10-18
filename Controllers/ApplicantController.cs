using JobSeeker.Models;
using JobSeeker.Repositories;
using JobSeeker.Services.FileStorageService;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class ApplicantController : Controller
    {

        private readonly ILogger<ApplicantController> _logger;
        private readonly ApplicantRepository applicantRepository = new ApplicantRepository();
        private readonly IFileStorageService _fileStorageService;

        public ApplicantController(ILogger<ApplicantController> logger, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public IActionResult Index()
        {
            var Applicants = applicantRepository.GetAllApplicants();
            return View(Applicants);
        }

        [HttpGet]
        public IActionResult Create()
        {
            SeekerRepository seekerRepository = new();
            ViewBag.Seekers = seekerRepository.GetAllSeekers();

            JobRepository jobRepository = new();
            ViewBag.Jobs = jobRepository.GetAllJobs();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Store(Applicant applicant)
        {
            applicant.AppliedAt = DateTime.Now;
            applicantRepository.Store(applicant);

            ViewBag.Message = "Create new applicant success";
            return RedirectToAction("Index", "Applicant");
        }

        public IActionResult Edit(int Id)
        {
            var Applicant = applicantRepository.GetApplicantById(Id);

            SeekerRepository seekerRepository = new();
            ViewBag.Seekers = seekerRepository.GetAllSeekers();

            JobRepository jobRepository = new();
            ViewBag.Jobs = jobRepository.GetAllJobs();

            return View(Applicant);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Applicant Applicant)
        {
            var isUpdated = applicantRepository.Update(Applicant);
            if (isUpdated)
            {
                return RedirectToAction("Index", "Applicant");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {

            Applicant? Applicant = applicantRepository.GetApplicantById(Id);

            if (Applicant != null)
            {
                applicantRepository.DeleteApplicantById(Id);
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
using JobSeeker.Models;
using JobSeeker.Repositories;
using Microsoft.AspNetCore.Mvc;
namespace JobSeeker.Controllers
{
    public class JobPositionController : Controller
    {
        private readonly JobPositionRepository jobPositionRepository = new JobPositionRepository();

        private readonly ILogger<JobPositionController> _logger;

        public JobPositionController(ILogger<JobPositionController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var JobPosition = jobPositionRepository.GetAllJobPositions();
            return View(JobPosition);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Store(JobPosition jobPosition)
        {
            ViewBag.Message = "Create new job position success";
            jobPositionRepository.Store(jobPosition);
            return RedirectToAction("Index", "JobPosition");
        }

        public IActionResult Edit(int Id)
        {
            var JobPosition = jobPositionRepository.GetJobPositionById(Id);
            return View(JobPosition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(JobPosition JobPosition)
        {

            if (ModelState.IsValid)
            {
                var isUpdated = jobPositionRepository.Update(JobPosition);
                if (isUpdated)
                {
                    return RedirectToAction("Index", "JobPosition");
                }
                ModelState.AddModelError("", "Failed to update job position.");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            jobPositionRepository.DeleteJobPositionById(Id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
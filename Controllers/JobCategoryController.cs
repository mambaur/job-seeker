
using JobSeeker.Models;
using JobSeeker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class JobCategoryController : Controller
    {
        private readonly JobCategoryRepository jobCategoryRepository = new JobCategoryRepository();

        private readonly ILogger<JobCategoryController> _logger;

        public JobCategoryController(ILogger<JobCategoryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var JobCategory = jobCategoryRepository.GetAllJobCategories();
            return View(JobCategory);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Store(JobCategory JobCategory)
        {
            ViewBag.Message = "Create new job category success";
            jobCategoryRepository.Store(JobCategory);
            return RedirectToAction("Index", "JobCategory");
        }

        public IActionResult Edit(int Id)
        {
            var JobCategory = jobCategoryRepository.GetJobCategoryById(Id);
            return View(JobCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(JobCategory JobCategory)
        {

            if (ModelState.IsValid)
            {
                var isUpdated = jobCategoryRepository.Update(JobCategory);
                if (isUpdated)
                {
                    return RedirectToAction("Index", "JobCategory");
                }
                ModelState.AddModelError("", "Failed to update job category.");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            jobCategoryRepository.DeleteJobCategoryById(Id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
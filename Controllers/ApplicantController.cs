using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class ApplicantController : Controller
    {

        private readonly ILogger<ApplicantController> _logger;

        public ApplicantController(ILogger<ApplicantController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
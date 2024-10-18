using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JobSeeker.Models;
using JobSeeker.Repositories;

namespace JobSeeker.Controllers;

public class HomeController : Controller
{

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        ApplicantRepository applicantRepository = new ApplicantRepository();
        ViewBag.TotalApplicant = applicantRepository.GetTotalApplicants();

        JobRepository jobRepository = new JobRepository();
        ViewBag.TotalJob = jobRepository.GetTotalJobs();

        SeekerRepository seekerRepository = new SeekerRepository();
        ViewBag.TotalSeeker = seekerRepository.GetTotalSeekers();

        RecruiterRepository recruiterRepository = new RecruiterRepository();
        ViewBag.TotalRecruiter = recruiterRepository.GetTotalRecruiters();

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

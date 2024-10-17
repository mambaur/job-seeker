using JobSeeker.Models;
using JobSeeker.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly RoleRepository roleRepository = new RoleRepository();

        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(ILogger<UserRoleController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var Roles = roleRepository.GetAllRoles();
            return View(Roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Store(UserRole role)
        {
            ViewBag.Message = "Create new role success";
            roleRepository.Store(role);
            return RedirectToAction("Index", "UserRole");
        }

        public IActionResult Edit(int Id)
        {
            var Role = roleRepository.GetRoleById(Id);
            return View(Role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UserRole UserRole)
        {

            if (ModelState.IsValid)
            {
                var isUpdated = roleRepository.Update(UserRole);
                if (isUpdated)
                {
                    return RedirectToAction("Index", "UserRole");
                }
                ModelState.AddModelError("", "Failed to update role.");
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            roleRepository.DeleteRoleById(Id);

            return Redirect(Request.Headers["Referer"].ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
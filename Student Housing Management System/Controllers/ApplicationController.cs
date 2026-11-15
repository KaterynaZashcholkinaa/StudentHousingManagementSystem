
using Microsoft.AspNetCore.Mvc;
using Student_Housing_Management_System.Services;

namespace Student_Housing_Management_System.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationService _applicationService;

        public ApplicationController(ApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public IActionResult Index()
        {
            var applications = _applicationService.GetAll();

            return View(applications);
        }

        public IActionResult Details(int id)
        {
            var application = _applicationService.GetById(id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }
    }
}

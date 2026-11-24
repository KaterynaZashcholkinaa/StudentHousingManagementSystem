using Microsoft.AspNetCore.Mvc;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Services;
using Student_Housing_Management_System.ViewModels;

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


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Application application)
        {
            if (ModelState.IsValid)
            {
                _applicationService.Add(application);
                return RedirectToAction("Index");
            }

            return View(application);
        }

        [HttpGet]
        public IActionResult EditStatus(int id)
        {
            ApplicationViewModel? application = _applicationService.GetById(id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStatus(int id, Application application)
        {
            if (id != application.ApplicationId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                bool updated = _applicationService.UpdateStatus(application.ApplicationId,application.ApplicationStatus);

                if (!updated)
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
            }

            ApplicationViewModel? existingApplication =_applicationService.GetById(id);

            if (existingApplication == null)
            {
                return NotFound();
            }

            existingApplication.ApplicationStatus = application.ApplicationStatus;

            return View(existingApplication);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            ApplicationViewModel? application = _applicationService.GetById(id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleted = _applicationService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }
    }
}

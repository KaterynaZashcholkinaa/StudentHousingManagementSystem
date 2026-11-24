using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Services;
using Student_Housing_Management_System.ViewModels;

namespace Student_Housing_Management_System.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly ApplicationService _applicationService;
        private readonly ILogger<ApplicationController> _logger;

        public ApplicationController(ApplicationService applicationService,ILogger<ApplicationController> logger)
        {
            _applicationService = applicationService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var applications = _applicationService.GetAll();

                return View(applications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving applications.");

                TempData["ErrorMessage"] = "Unable to load applications. Please try again.";

                return View(new List<ApplicationViewModel>());
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                var application = _applicationService.GetById(id);

                if (application == null)
                {
                    return NotFound();
                }

                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving application with ID {ApplicationId}.", id);

                TempData["ErrorMessage"] = "Unable to load application details. Please try again.";

                return RedirectToAction("Index");
            }
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
            if (!ModelState.IsValid)
            {
                return View(application);
            }

            try
            {
                _applicationService.Add(application);

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating an application.");

                TempData["ErrorMessage"] = "Unable to save the application. Please check the selected student and room.";

                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating an application.");

                TempData["ErrorMessage"] ="Something went wrong while creating the application.";

                return View(application);
            }
        }

        [HttpGet]
        public IActionResult EditStatus(int id)
        {
            try
            {
                ApplicationViewModel? application =
                    _applicationService.GetById(id);

                if (application == null)
                {
                    return NotFound();
                }

                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while retrieving application with ID {ApplicationId}.", id);

                TempData["ErrorMessage"] = "Unable to load the application. Please try again.";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditStatus(int id, Application application)
        {
            if (id != application.ApplicationId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    ApplicationViewModel? existingApplication = _applicationService.GetById(id);

                    if (existingApplication == null)
                    {
                        return NotFound();
                    }

                    existingApplication.ApplicationStatus = application.ApplicationStatus;

                    return View(existingApplication);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while retrieving application with ID {ApplicationId}.", id);

                    TempData["ErrorMessage"] = "Unable to load the application. Please try again.";

                    return RedirectToAction("Index");
                }
            }

            try
            {
                bool updated = _applicationService.UpdateStatus(application.ApplicationId,application.ApplicationStatus);

                if (!updated)
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,"Database error while updating application with ID {ApplicationId}.", id);

                TempData["ErrorMessage"] ="Unable to update the application status. Please try again.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while updating application with ID {ApplicationId}.", id);

                TempData["ErrorMessage"] ="Something went wrong while updating the application status.";

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                ApplicationViewModel? application = _applicationService.GetById(id);

                if (application == null)
                {
                    return NotFound();
                }

                return View(application);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while retrieving application with ID {ApplicationId}.", id);

                TempData["ErrorMessage"] ="Unable to load the application. Please try again.";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = _applicationService.Delete(id);

                if (!deleted)
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,"Database error while deleting application with ID {ApplicationId}.", id);

                TempData["ErrorMessage"] ="Unable to delete the application because it may be referenced by other records.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while deleting application with ID {ApplicationId}.", id);

                TempData["ErrorMessage"] ="Something went wrong while deleting the application.";

                return RedirectToAction("Index");
            }
        }
    }
}

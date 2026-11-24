
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
                SetErrorMessage(ex,"Error while retrieving applications.","Unable to load applications. Please try again.");

                return View(new List<ApplicationViewModel>());
            }
        }

        public IActionResult Details(int id)
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
                return HandleLoadError(ex, id,"Unable to load application details. Please try again.");
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

                TempData["SuccessMessage"] = "Application was successfully created.";

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                return HandleFormError(application, ex,"Database error while creating an application.","Unable to save the application. Please check the selected student and room.");
            }
            catch (Exception ex)
            {
                return HandleFormError(application, ex,"Error while creating an application.","Something went wrong while creating the application.");
            }
        }

        [HttpGet]
        public IActionResult EditStatus(int id)
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
                return HandleLoadError(ex, id,"Unable to load the application. Please try again.");
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
                    ApplicationViewModel? existingApplication =
                        _applicationService.GetById(id);

                    if (existingApplication == null)
                    {
                        return NotFound();
                    }

                    existingApplication.ApplicationStatus = application.ApplicationStatus;

                    return View(existingApplication);
                }
                catch (Exception ex)
                {
                    return HandleLoadError(ex, id,"Unable to load the application. Please try again.");
                }
            }

            try
            {
                bool updated = _applicationService.UpdateStatus(application.ApplicationId,application.ApplicationStatus);

                if (!updated)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Application status was successfully updated.";

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                SetErrorMessage(ex,$"Database error while updating application with ID {id}.","Unable to update the application status. Please try again.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex,$"Error while updating application with ID {id}.","Something went wrong while updating the application status.");

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
                return HandleLoadError(ex, id,"Unable to load the application. Please try again.");
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

                TempData["SuccessMessage"] = "Application was successfully deleted.";

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                SetErrorMessage(ex, $"Database error while deleting application with ID {id}.", "Unable to delete the application because it may be referenced by other records.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex, $"Error while deleting application with ID {id}.", "Something went wrong while deleting the application.");

                return RedirectToAction("Index");
            }
        }

        private void SetErrorMessage(Exception ex, string logMessage, string userMessage)
        {
            _logger.LogError(ex, logMessage);
            TempData["ErrorMessage"] = userMessage;
        }

        private IActionResult HandleLoadError(Exception ex, int id, string userMessage)
        {
            SetErrorMessage(ex,
                $"Error while retrieving application with ID {id}.",
                userMessage);

            return RedirectToAction("Index");
        }

        private IActionResult HandleFormError(Application application,Exception ex,string logMessage,string userMessage)
        {
            SetErrorMessage(ex, logMessage, userMessage);

            return View(application);
        }
    }
}

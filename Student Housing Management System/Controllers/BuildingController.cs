using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Services;

namespace Student_Housing_Management_System.Controllers
{
    public class BuildingController : Controller
    {
        private readonly BuildingService _buildingService;
        private readonly ILogger<BuildingController> _logger;

        public BuildingController(BuildingService buildingService,ILogger<BuildingController> logger)
        {
            _buildingService = buildingService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                List<Building> buildings = _buildingService.GetAll();

                return View(buildings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving buildings.");

                TempData["ErrorMessage"] ="Unable to load buildings. Please try again.";

                return View(new List<Building>());
            }
        }

        public IActionResult Details(int id)
        {
            try
            {
                Building? building = _buildingService.GetById(id);

                if (building == null)
                {
                    return NotFound();
                }

                return View(building);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while retrieving building with ID {BuildingId}.", id);

                TempData["ErrorMessage"] ="Unable to load building details. Please try again.";

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
        public IActionResult Create(Building building)
        {
            if (!ModelState.IsValid)
            {
                return View(building);
            }

            try
            {
                _buildingService.Add(building);

                TempData["SuccessMessage"] = "Building created successfully.";

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating a building.");

                TempData["ErrorMessage"] = "Unable to save the building. Please check the information and try again.";

                return View(building);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a building.");

                TempData["ErrorMessage"] = "Something went wrong while creating the building.";

                return View(building);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                Building? building = _buildingService.GetById(id);

                if (building == null)
                {
                    return NotFound();
                }

                return View(building);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while retrieving building with ID {BuildingId}.", id);

                TempData["ErrorMessage"] ="Unable to load the building. Please try again.";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Building building)
        {
            if (!ModelState.IsValid)
            {
                return View(building);
            }

            try
            {
                bool updated = _buildingService.Update(building);

                if (!updated)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Building updated successfully.";

                return RedirectToAction("Details",new { id = building.BuildingId });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,"Database error while updating building with ID {BuildingId}.",building.BuildingId);

                TempData["ErrorMessage"] ="Unable to update the building. Please try again.";

                return View(building);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while updating building with ID {BuildingId}.",building.BuildingId);

                TempData["ErrorMessage"] = "Something went wrong while updating the building.";

                return View(building);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                Building? building = _buildingService.GetById(id);

                if (building == null)
                {
                    return NotFound();
                }

                return View(building);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while retrieving building with ID {BuildingId}.", id);

                TempData["ErrorMessage"] ="Unable to load the building. Please try again.";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = _buildingService.Delete(id);

                if (!deleted)
                {
                    TempData["ErrorMessage"] ="The building cannot be deleted because it does not exist or still has rooms.";

                    return RedirectToAction("Details", new { id });
                }

                TempData["SuccessMessage"] = "Building deleted successfully.";

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,"Database error while deleting building with ID {BuildingId}.", id);

                TempData["ErrorMessage"] ="Unable to delete the building because it may contain related records.";

                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while deleting building with ID {BuildingId}.", id);

                TempData["ErrorMessage"] ="Something went wrong while deleting the building.";

                return RedirectToAction("Index");
            }
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Services;

namespace Student_Housing_Management_System.Controllers
{
    public class BuildingController : Controller
    {
        private readonly BuildingService _buildingService;

        public BuildingController(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public IActionResult Index()
        {
            List<Building> buildings = _buildingService.GetAll();
            return View(buildings);
        }

        public IActionResult Details(int id)
        {
            Building? building = _buildingService.GetById(id);

            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

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

            _buildingService.Add(building);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            Building? building = _buildingService.GetById(id);

            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Building building)
        {
            if (!ModelState.IsValid)
            {
                return View(building);
            }

            bool updated = _buildingService.Update(building);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = building.BuildingId });
        }

        public IActionResult Delete(int id)
        {
            Building? building = _buildingService.GetById(id);

            if (building == null)
            {
                return NotFound();
            }

            return View(building);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleted = _buildingService.Delete(id);

            if (!deleted)
            {
                TempData["Error"] = "The building cannot be deleted because it still has rooms.";
                return RedirectToAction("Details", new { id });
            }

            return RedirectToAction("Index");
        }
    }
}
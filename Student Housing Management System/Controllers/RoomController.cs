using Microsoft.AspNetCore.Mvc;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Services;

namespace Student_Housing_Management_System.Controllers
{
    public class RoomController : Controller
    {
        private readonly RoomService _roomService;
        private readonly BuildingService _buildingService;

        public RoomController(
            RoomService roomService,
            BuildingService buildingService)
        {
            _roomService = roomService;
            _buildingService = buildingService;
        }

        public IActionResult Create(int buildingId)
        {
            Building? building = _buildingService.GetById(buildingId);

            if (building == null)
            {
                return NotFound();
            }

            Room room = new Room();
            room.BuildingId = buildingId;

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            Building? building = _buildingService.GetById(room.BuildingId);

            if (building == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(room);
            }

            _roomService.Add(room);

            return RedirectToAction("Details", "Building",
                new { id = room.BuildingId });
        }

        public IActionResult Edit(int id)
        {
            Room? room = _roomService.GetById(id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Room room)
        {
            Room? existingRoom = _roomService.GetById(room.RoomId);

            if (existingRoom == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                room.BuildingId = existingRoom.BuildingId;
                return View(room);
            }

            room.BuildingId = existingRoom.BuildingId;

            bool updated = _roomService.Update(room);

            if (!updated)
            {
                return NotFound();
            }

            return RedirectToAction("Details", "Building",
                new { id = room.BuildingId });
        }

        public IActionResult Delete(int id)
        {
            Room? room = _roomService.GetById(id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            Room? room = _roomService.GetById(id);

            if (room == null)
            {
                return NotFound();
            }

            int buildingId = room.BuildingId;

            bool deleted = _roomService.Delete(id);

            if (!deleted)
            {
                TempData["Error"] = "This room cannot be deleted because it has applications.";
            }

            return RedirectToAction("Details", "Building",
                new { id = buildingId });
        }
    }
}

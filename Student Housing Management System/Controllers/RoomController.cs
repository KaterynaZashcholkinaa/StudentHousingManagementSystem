using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Services;

namespace Student_Housing_Management_System.Controllers
{
    public class RoomController : Controller
    {
        private readonly RoomService _roomService;
        private readonly BuildingService _buildingService;
        private readonly ILogger<RoomController> _logger;

        public RoomController(RoomService roomService,BuildingService buildingService,ILogger<RoomController> logger)
        {
            _roomService = roomService;
            _buildingService = buildingService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Create(int buildingId)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while preparing room creation for building with ID {BuildingId}.",buildingId);

                TempData["ErrorMessage"] = "Unable to load the room creation page. Please try again.";

                return RedirectToAction("Details", "Building",new { id = buildingId });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Room room)
        {
            try
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

                TempData["SuccessMessage"] = "Room created successfully.";

                return RedirectToAction("Details", "Building",new { id = room.BuildingId });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,"Database error while creating a room in building with ID {BuildingId}.",room.BuildingId);

                TempData["ErrorMessage"] ="Unable to save the room. Please check the information and try again.";

                return View(room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while creating a room in building with ID {BuildingId}.",room.BuildingId);

                TempData["ErrorMessage"] ="Something went wrong while creating the room.";

                return View(room);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                Room? room = _roomService.GetById(id);

                if (room == null)
                {
                    return NotFound();
                }

                return View(room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while retrieving room with ID {RoomId}.", id);

                TempData["ErrorMessage"] ="Unable to load the room. Please try again.";

                return RedirectToAction("Index", "Building");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Room room)
        {
            try
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

                TempData["SuccessMessage"] = "Room updated successfully.";

                return RedirectToAction("Details", "Building", new { id = room.BuildingId });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating room with ID {RoomId}.",room.RoomId);

                TempData["ErrorMessage"] = "Unable to update the room. Please try again.";

                return View(room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while updating room with ID {RoomId}.", room.RoomId);

                TempData["ErrorMessage"] ="Something went wrong while updating the room.";

                return View(room);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                Room? room = _roomService.GetById(id);

                if (room == null)
                {
                    return NotFound();
                }

                return View(room);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error while retrieving room with ID {RoomId}.", id);

                TempData["ErrorMessage"] = "Unable to load the room. Please try again.";

                return RedirectToAction("Index", "Building");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
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
                    TempData["ErrorMessage"] ="This room cannot be deleted because it has related applications.";

                    return RedirectToAction("Details", "Building",
                        new { id = buildingId });
                }

                TempData["SuccessMessage"] = "Room deleted successfully.";

                return RedirectToAction("Details", "Building", new { id = buildingId });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex,"Database error while deleting room with ID {RoomId}.", id);

                TempData["ErrorMessage"] = "Unable to delete this room because it may have related applications.";

                return RedirectToAction("Index", "Building");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting room with ID {RoomId}.", id);

                TempData["ErrorMessage"] = "Something went wrong while deleting the room.";

                return RedirectToAction("Index", "Building");
            }
        }
    }
}

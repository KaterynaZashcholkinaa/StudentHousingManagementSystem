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

                Room room = new Room
                {
                    BuildingId = buildingId
                };

                return View(room);
            }
            catch (Exception ex)
            {
                return HandleLoadError(ex, $"Error while preparing room creation for building with ID {buildingId}.", "Unable to load the room creation page. Please try again.", buildingId);
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
                return HandleFormError(room, ex, $"Database error while creating a room in building with ID {room.BuildingId}.", "Unable to save the room. Please check the information and try again.");
            }
            catch (Exception ex)
            {
                return HandleFormError(room, ex,  $"Error while creating a room in building with ID {room.BuildingId}.", "Something went wrong while creating the room.");
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
                return HandleLoadError(ex, $"Error while retrieving room with ID {id}.",  "Unable to load the room. Please try again.");
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

                room.BuildingId = existingRoom.BuildingId;

                if (!ModelState.IsValid)
                {
                    return View(room);
                }

                bool updated = _roomService.Update(room);

                if (!updated)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Room updated successfully.";

                return RedirectToAction("Details", "Building",new { id = room.BuildingId });
            }
            catch (DbUpdateException ex)
            {
                return HandleFormError(room, ex, $"Database error while updating room with ID {room.RoomId}.", "Unable to update the room. Please try again.");
            }
            catch (Exception ex)
            {
                return HandleFormError(room, ex,$"Error while updating room with ID {room.RoomId}.","Something went wrong while updating the room.");
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
                return HandleLoadError(ex, $"Error while retrieving room with ID {id}.", "Unable to load the room. Please try again.");
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
                    TempData["ErrorMessage"] = "This room cannot be deleted because it has related applications.";

                    return RedirectToAction("Details", "Building",
                        new { id = buildingId });
                }

                TempData["SuccessMessage"] = "Room deleted successfully.";

                return RedirectToAction("Details", "Building",  new { id = buildingId });
            }
            catch (DbUpdateException ex)
            {
                SetErrorMessage(ex, $"Database error while deleting room with ID {id}.", "Unable to delete this room because it may have related applications.");

                return RedirectToAction("Index", "Building");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex, $"Error while deleting room with ID {id}.", "Something went wrong while deleting the room.");

                return RedirectToAction("Index", "Building");
            }
        }

        private void SetErrorMessage(Exception ex, string logMessage, string userMessage)
        {
            _logger.LogError(ex, logMessage);
            TempData["ErrorMessage"] = userMessage;
        }

        private IActionResult HandleLoadError(Exception ex,string logMessage,string userMessage,int? buildingId = null)
        {
            SetErrorMessage(ex, logMessage, userMessage);

            if (buildingId.HasValue)
            {
                return RedirectToAction("Details", "Building",
                    new { id = buildingId.Value });
            }

            return RedirectToAction("Index", "Building");
        }

        private IActionResult HandleFormError( Room room,Exception ex,string logMessage,string userMessage)
        {
            SetErrorMessage(ex, logMessage, userMessage);

            return View(room);
        }
    }
}

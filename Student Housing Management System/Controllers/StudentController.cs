using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Services;

namespace Student_Housing_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private readonly ILogger<StudentController> _logger;

        public StudentController(StudentService studentService,ILogger<StudentController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                List<Student> students = _studentService.GetAll();

                return View(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving students.");

                TempData["ErrorMessage"] = "Unable to load students. Please try again.";

                return View(new List<Student>());
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                _studentService.Add(student);

                TempData["SuccessMessage"] = "Student was successfully created.";

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while creating a student.");

                TempData["ErrorMessage"] = "Unable to save the student. Please check the information and try again.";

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating a student.");

                TempData["ErrorMessage"] = "Something went wrong while creating the student.";

                return View(student);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                Student? student = _studentService.GetById(id);

                if (student == null)
                {
                    return NotFound();
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving student with ID {StudentId}.", id);

                TempData["ErrorMessage"] = "Unable to load the student. Please try again.";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(student);
            }

            try
            {
                bool updated = _studentService.Update(student);

                if (!updated)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Student was successfully updated.";

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while updating student with ID {StudentId}.", id);

                TempData["ErrorMessage"] = "Unable to update the student. Please try again.";

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating student with ID {StudentId}.", id);

                TempData["ErrorMessage"] = "Something went wrong while updating the student.";

                return View(student);
            }
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                Student? student = _studentService.GetById(id);

                if (student == null)
                {
                    return NotFound();
                }

                return View(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving student with ID {StudentId}.", id);

                TempData["ErrorMessage"] = "Unable to load the student. Please try again.";

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = _studentService.Delete(id);

                if (!deleted)
                {
                    return NotFound();
                }

                TempData["SuccessMessage"] = "Student was successfully deleted.";

                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Database error while deleting student with ID {StudentId}.", id);

                TempData["ErrorMessage"] = "Unable to delete this student because they may have related applications.";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting student with ID {StudentId}.", id);

                TempData["ErrorMessage"] = "Something went wrong while deleting the student.";

                return RedirectToAction("Index");
            }
        }
    }
}

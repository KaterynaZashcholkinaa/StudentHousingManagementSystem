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
                SetErrorMessage(ex, "Error while retrieving students.","Unable to load students. Please try again.");

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
                return HandleFormError(student, ex,"Database error while creating a student.","Unable to save the student. Please check the information and try again.");
            }
            catch (Exception ex)
            {
                return HandleFormError(student, ex,"Error while creating a student.","Something went wrong while creating the student.");
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
                return HandleLoadError(ex, id);
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
                return HandleFormError(student, ex,$"Database error while updating student with ID {id}.","Unable to update the student. Please try again.");
            }
            catch (Exception ex)
            {
                return HandleFormError(student, ex, $"Error while updating student with ID {id}.", "Something went wrong while updating the student.");
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
                return HandleLoadError(ex, id);
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
                SetErrorMessage(ex, $"Database error while deleting student with ID {id}.","Unable to delete this student because they may have related applications.");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                SetErrorMessage(ex,$"Error while deleting student with ID {id}.", "Something went wrong while deleting the student.");

                return RedirectToAction("Index");
            }
        }

        private void SetErrorMessage(Exception ex, string logMessage, string userMessage)
        {
            _logger.LogError(ex, logMessage);
            TempData["ErrorMessage"] = userMessage;
        }

        private IActionResult HandleLoadError(Exception ex, int id)
        {
            SetErrorMessage(ex,$"Error while retrieving student with ID {id}.","Unable to load the student. Please try again.");

            return RedirectToAction("Index");
        }

        private IActionResult HandleFormError(Student student,Exception ex,string logMessage,string userMessage)
        {
            SetErrorMessage(ex, logMessage, userMessage);

            return View(student);
        }
    }
}

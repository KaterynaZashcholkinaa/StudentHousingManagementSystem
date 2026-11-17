using Microsoft.AspNetCore.Mvc;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Repositories;

namespace Student_Housing_Management_System.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository _studentRepository;

        public StudentController(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IActionResult Index()
        {
            List<Student> students = _studentRepository.GetAll();

            return View(students);
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
            if (ModelState.IsValid)
            {
                _studentRepository.Add(student);

                return RedirectToAction("Index");
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student? student = _studentRepository.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                bool updated = _studentRepository.Update(student);

                if (!updated)
                {
                    return NotFound();
                }

                return RedirectToAction("Index");
            }

            return View(student);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Student? student = _studentRepository.GetById(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleted = _studentRepository.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return RedirectToAction("Index");
        }
    }
}

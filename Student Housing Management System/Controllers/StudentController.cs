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
    }
}

using Microsoft.AspNetCore.Mvc;
using Student_Housing_Management_System.Models;
using Student_Housing_Management_System.Repositories;

namespace Student_Housing_Management_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly StudentRepository _studentRepository;

        public HomeController(StudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IActionResult Index()
        {
            List<Student> students = _studentRepository.GetAll();

            return View(students);
        }
    }
}   
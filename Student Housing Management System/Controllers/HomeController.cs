using Microsoft.AspNetCore.Mvc;

namespace Student_Housing_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            List<Student> students = _studentRepository.GetAll();

            return View(students);
        }
    }
}   
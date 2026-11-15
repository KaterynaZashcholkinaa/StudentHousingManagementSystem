using Microsoft.AspNetCore.Mvc;

namespace Student_Housing_Management_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}   
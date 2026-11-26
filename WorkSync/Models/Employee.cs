using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Models
{
    public class Employee : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

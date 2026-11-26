using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Models
{
    public class Department : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

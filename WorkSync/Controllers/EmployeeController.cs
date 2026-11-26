using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

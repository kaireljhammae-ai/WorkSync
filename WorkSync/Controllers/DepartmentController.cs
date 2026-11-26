using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

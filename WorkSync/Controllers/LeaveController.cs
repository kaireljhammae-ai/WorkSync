using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Controllers
{
    public class LeaveController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

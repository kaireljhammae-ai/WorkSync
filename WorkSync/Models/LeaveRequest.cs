using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Models
{
    public class LeaveRequest : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

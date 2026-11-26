using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Models
{
    public class OvertimeRequest : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

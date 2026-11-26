using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Models
{
    public class UndertimeRequest : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

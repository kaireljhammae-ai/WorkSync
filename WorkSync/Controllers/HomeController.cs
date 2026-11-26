using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Data
{
    public class DbInitializer : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

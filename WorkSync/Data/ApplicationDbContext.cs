using Microsoft.AspNetCore.Mvc;

namespace WorkSync.Data
{
    public class ApplicationDbContext : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
